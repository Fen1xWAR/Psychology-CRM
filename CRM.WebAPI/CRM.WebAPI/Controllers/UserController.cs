using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        // POST: api/User/Register
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RegisterAsync([FromBody] UserAuth model)
        {
            var user = await _repository.CreateUser(model);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }

        // POST: api/User/Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserAuth model)
        {
            var result = await _authService.GenerateTokenAsync(model);
            var token = result.Result;
            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { token });
            }

            return BadRequest(result.ErrorMessage);
        }


        [Authorize(Policy = "Admin")]
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
            return Ok(await _repository.GetById(id));
        }


        // PUT: api/User/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] User user)
        {
            await _repository.Put(user);
            return Ok();
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody] User dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _repository.RemoveById(id);
            return Ok();
        }
    }
}