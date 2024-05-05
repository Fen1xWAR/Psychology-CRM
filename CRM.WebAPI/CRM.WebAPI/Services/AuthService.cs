using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private IFormRepository _formRepository;
    private IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, IContactRepository contactRepository, IClientRepository clientRepository, IPsychologistRepository psychologistRepository, IFormRepository formRepository)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _contactRepository = contactRepository;
        _clientRepository = clientRepository;
        _psychologistRepository = psychologistRepository;
        _formRepository = formRepository;
    }

    public async Task<IOperationResult<string>> Login(UserAuth model)
    {
        var user = await _userRepository.GetUserByEmail(model.Email);

        if (!user.Successful || (user.Result.Password != model.Password))
            return new ConflictResult<string>(null, "Invalid login or password");
        return new Success<string>(GenerateTokenAsync(new UserBase()
        {
            UserId = user.Result.UserId,
            Role = user.Result.Role
        }));

    }

    public async Task<IOperationResult<string>> Register(UserRegModel userRegModel)
    {
        var user = await _userRepository.GetUserByEmail(userRegModel.Email);
        if(user.Successful)
            return new ConflictResult<string>(null, "User with this email is already exist");
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
        return new Success<string>(GenerateTokenAsync(new UserBase
        {
            UserId = userId,
            Role = userRegModel.Role
        }));
    }

    private string GenerateTokenAsync(UserBase userData)
    { 
        
        
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