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
        var user = await _userRepository.GetUserByEmail(model.Email);

        if (!user.Successful || (user.Result.Password != model.Password))
            return new ConflictResult<Tokens>(null, "Invalid login or password");
        var userId = user.Result.UserId;
        var tokens = GenerateTokensAsync(new UserBase()
        {
            UserId = userId,
            Role = user.Result.Role
        });
        await _tokenRepository.UpdateToken(new TokenModel()
        {
            userId = userId,
            RefreshToken = tokens.RefreshToken
        });
        return new Success<Tokens>(tokens);
    }

    public async Task<IOperationResult<Tokens>> RefreshTokens(string token, HttpContext context)
    {
        var user = GetCurrentUser(context);
        if (user == null)
            return new ElementNotFound<Tokens>(null, "JWT Token not found!");

        var existingToken = await _tokenRepository.GetTokenByUserId(user.UserId);
        if (!existingToken.Successful)
            return new ElementNotFound<Tokens>(null, "RefreshToken not found in base!");
        if (existingToken.Result.RefreshToken != token)
            return new ElementNotFound<Tokens>(null, "Invalid refresh token!");
        var tokens = GenerateTokensAsync(user);
        await _tokenRepository.UpdateToken(new TokenModel()
        {
            userId = user.UserId,
            RefreshToken = tokens.RefreshToken
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
            Lastname = userRegModel.LastName
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

        var tokens = GenerateTokensAsync(new UserBase
        {
            UserId = userId,
            Role = userRegModel.Role
        });
        await _tokenRepository.WriteTokenAsync(new TokenModel()
        {
            userId = userId,
            RefreshToken = tokens.RefreshToken
        });
        return new Success<Tokens>(tokens);
    }

    private Tokens GenerateTokensAsync(UserBase userData)
    {
        return new Tokens
        {
            JWTToken = GenerateJwtToken(userData),
            RefreshToken = GenerateRefreshToken()
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

    private string GenerateRefreshToken()
    {
        var _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        byte[] tokenBuffer = new byte[64];
        _rngCryptoServiceProvider.GetBytes(tokenBuffer);
        return Convert.ToBase64String(tokenBuffer);
    }


    public UserBase? GetCurrentUser(HttpContext user)
    {
        var userIdClaim = user.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        var roleClaim = user.User.FindFirst(c => c.Type == ClaimTypes.Role);

        if (userIdClaim == null || roleClaim == null)
            return null;
        return new UserBase()
        {
            UserId = Guid.Parse(userIdClaim.Value),
            Role = roleClaim.Value,
        };
    }
}