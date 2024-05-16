using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRM.Domain.Models;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.ModelsToUpload;
using CRM.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Serilog;


namespace CRM.WebAPI.Services;

public class AuthService : IAuthService
{
    private IUserRepository _userRepository;
    private IContactRepository _contactRepository;
    private IClientRepository _clientRepository;
    private IPsychologistRepository _psychologistRepository;
    private ITokenRepository _tokenRepository;
    private IFormRepository _formRepository;
    private IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration,
        IContactRepository contactRepository, IClientRepository clientRepository,
        IPsychologistRepository psychologistRepository, IFormRepository formRepository,
        ITokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _contactRepository = contactRepository;
        _clientRepository = clientRepository;
        _psychologistRepository = psychologistRepository;
        _formRepository = formRepository;
        _tokenRepository = tokenRepository;
    }

    public async Task<IOperationResult<Tokens>> Login(UserAuth model)
    {
        Log.Logger.Error(model.DeviceId.ToString());
        var user = await _userRepository.GetUserByEmail(model.Email);

        if (!user.Successful || user.Result.Password != model.Password)
            return new ConflictResult<Tokens>(null, "Invalid login or password");
        Guid userId = user.Result.UserId;

        if (model.DeviceId != Guid.Empty)
        {
            Log.Logger.Error("Deleting token");
            await _tokenRepository.RemoveTokenByUserAndDeviceId(userId, model.DeviceId);
        }

        var deviceId = Guid.NewGuid();
        var tokens = GenerateTokensAsync(new UserBase()
        {
            UserId = userId,
            Role = user.Result.Role
        }, deviceId);
        await _tokenRepository.WriteTokenAsync(new TokenModel()
        {
            UserId = userId,
            RefreshToken = tokens.RefreshToken.Token,
            DeviceId = deviceId,
            ExpiredDateTime = DateTime.Now.AddDays(7)
        });

        return new Success<Tokens>(tokens);
    }

    public async Task<IOperationResult<Tokens>> RefreshTokens(Tokens token)
    {
        var user = ParseJwtToken(token.JWTToken);
        if (user == null || token.RefreshToken == null)
            return new ElementNotFound<Tokens>(null, "Some token is null!");
        var existingToken = await _tokenRepository.GetTokenByUserAndDeviceId(user.UserId, token.RefreshToken.DeviceId);
        if (!existingToken.Successful)
            return new ElementNotFound<Tokens>(null, "RefreshToken not found in base!");
        if (existingToken.Result.RefreshToken != token.RefreshToken.Token ||
            (existingToken.Result.ExpiredDateTime < DateTime.Now))
        {
            Log.Logger.Fatal($"Было:{existingToken.Result.RefreshToken} ==  С фронта:{token.RefreshToken.Token}");
            await _tokenRepository.RemoveAllUserTokens(user.UserId);
            return new ElementNotFound<Tokens>(null, "Invalid refresh token!");
        }

        var deviceId = Guid.NewGuid();
        var tokens = GenerateTokensAsync(user, deviceId);
        await _tokenRepository.RemoveTokenByUserAndDeviceId(user.UserId, token.RefreshToken.DeviceId);
        await _tokenRepository.WriteTokenAsync(new TokenModel()
        {
            UserId = user.UserId,
            RefreshToken = tokens.RefreshToken.Token,
            DeviceId = deviceId
        });
        return new Success<Tokens>(tokens);
    }

    public async Task<IOperationResult<Tokens>> Register(UserRegModel userRegModel)
    {
        var user = await _userRepository.GetUserByEmail(userRegModel.Email);
        if (user.Successful)
            return new ConflictResult<Tokens>(null, "User with this email is already exist");
        var contactId = (await _contactRepository.Put(new ContactModel
        {
            Name = userRegModel.Name,
            Lastname = userRegModel.LastName,
            DateOfBirth= userRegModel.DateOfBirth
        })).Result;
        var userId = (await _userRepository.Put(new UserModel
        {
            Email = userRegModel.Email,
            Password = userRegModel.Password,
            ContactId = contactId,
            Role = userRegModel.Role
        })).Result;

        switch (userRegModel.Role)
        {
            case "Admin":
                break;
            case "Client":
                var formId = (await _formRepository.Put(new FormModel())).Result;
                await _clientRepository.Put(new ClientModel()
                {
                    FormId = formId,
                    UserId = userId
                });
                break;
            case "Psychologist":
                await _psychologistRepository.Put(new PsychologistModel()
                {
                    UserId = userId
                });
                break;
            default:
                throw new ArgumentException("CurrentRoleNotImplement!");
        }

        var deviceId = Guid.NewGuid();
        var tokens = GenerateTokensAsync(new UserBase
        {
            UserId = userId,
            Role = userRegModel.Role
        }, deviceId);
        await _tokenRepository.WriteTokenAsync(new TokenModel()
        {
            UserId = userId,
            RefreshToken = tokens.RefreshToken.Token,
            DeviceId = deviceId
        });
        return new Success<Tokens>(tokens);
    }

    private Tokens GenerateTokensAsync(UserBase userData, Guid deviceId)
    {
        return new Tokens
        {
            JWTToken = GenerateJwtToken(userData),
            RefreshToken = GenerateRefreshToken(deviceId)
        };
    }


    private string GenerateJwtToken(UserBase userData)
    {
        //JWT TOKEN
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userData.UserId.ToString()),
            new Claim(ClaimTypes.Role, userData.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthOptions:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["AuthOptions:Issuer"],
            audience: _configuration["AuthOptions:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(Guid deviceId)
    {
        var _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        byte[] tokenBuffer = new byte[64];
        _rngCryptoServiceProvider.GetBytes(tokenBuffer);
        return new RefreshToken()
        {
            Token = Convert.ToBase64String(tokenBuffer),
            DeviceId = deviceId
        };
    }


    public JwtSecurityToken GetJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken;

        try
        {
            jwtToken = handler.ReadJwtToken(token);
        }
        catch (Exception)
        {
            // If the token is not in a valid format, return null
            return null;
        }

        if (jwtToken == null)
        {
            return null;
        }

        return jwtToken;
    }

    public UserBase? ParseJwtToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var token = handler.ReadJwtToken(jwtToken);
            var userIdClaim = token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            var roleClaim = token.Claims.First(c => c.Type == ClaimTypes.Role);
            return new UserBase()
            {
                UserId = Guid.Parse(userIdClaim.Value),
                Role = roleClaim.Value,
            };
        }
        catch
        {
            return null;
        }
    }

    public async Task<IOperationResult<User>> GetCurrentUserIternal(HttpContext context)
    {
        var userBase = GetCurrentUser(context);
        if (!userBase.Successful)
            return new ConflictResult<User>(null, userBase.ErrorMessage);
        return  await _userRepository.GetById(userBase.Result.UserId);

    }
    public IOperationResult<UserBase> GetCurrentUser(HttpContext user)
    {
        var token = GetJwtToken(user.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (token != null)
        {
            var userIdClaim = token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            var roleClaim = token.Claims.First(c => c.Type == ClaimTypes.Role);
            return new Success<UserBase>(new UserBase()
            {
                UserId = Guid.Parse(userIdClaim.Value),
                Role = roleClaim.Value,
            });
        }
        else
        {
            return new ConflictResult<UserBase>(null, "TokenNotFound");
        }
    }
}