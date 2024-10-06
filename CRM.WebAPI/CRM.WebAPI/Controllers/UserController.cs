using System.Security.Claims;
using System.Web;
using CRM.Core.Implement;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConflictResult = CRM.Core.Implement.ConflictResult;

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserRepository _repository;
        private IAuthService _authService;

        public UserController(IUserRepository repository, IAuthService authService)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _authService = authService ?? throw new ArgumentException(nameof(authService));
        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserRegModel regModel)
        {
            if (regModel.Email == "" || regModel.Password == "" || regModel.Role == "")
                return BadRequest(new ConflictResult("Необходимо заполнить обязательные поля"));
            if (!(regModel.Role is "Admin" or "Client" or "Psychologist"))
                return BadRequest(new ConflictResult($"Cant create user with role {regModel.Role}"));
            var tokens = await _authService.Register(regModel);
            if (!tokens.Successful)
                return BadRequest(tokens);
            var refreshToken = tokens.Result.RefreshToken;
            // WriteRefreshToCookie(refreshToken);
            return Ok(tokens);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserAuth model)
        {
            if (model.Email == null || model.Password == null)
                return BadRequest(new ConflictResult("Необходимо заполнить все поля"));
            var tokens = await _authService.Login(model);
            if (!tokens.Successful)
                return BadRequest(tokens);
            var refreshToken = tokens.Result.RefreshToken;

            // WriteRefreshToCookie(refreshToken);
            return Ok(tokens);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] Tokens token)
        {

            if (token.JWTToken == "" || token.RefreshToken == null || token.RefreshToken.Token == "" ||
                token.RefreshToken.DeviceId == Guid.Empty)
                return Unauthorized("Invalid Input");
            var result = await _authService.RefreshTokens(token);
            if (!result.Successful)
            {
                return Unauthorized(result);
            }

            // WriteRefreshToCookie(result.Result.RefreshToken);

            return Ok(result);
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ConflictResult("Пустой ввод не допустим"));
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);

            return NotFound(result);
        }


        // PUT: api/User/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] UserModel user)
        {
            var result = await _repository.Put(user);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody] User dataToUpdate)
        {
            if (dataToUpdate.UserId == Guid.Empty)
                return BadRequest(new ConflictResult("Пустой ввод не допустим"));
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ConflictResult("Пустой ввод не допустим"));
            var result = await _repository.RemoveById(id);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

       

        private void WriteRefreshToCookie(string refreshToken)
        {
            HttpContext.Response.Cookies.Delete("refreshToken");
            var encodedRefreshToken = HttpUtility.UrlEncode(refreshToken);
            HttpContext.Response.Cookies.Append("refreshToken", encodedRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}