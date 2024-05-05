using System.Security.Claims;
using CRM.Domain.Models;
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


        // POST: api/User/Register
        // [AllowAnonymous]
        // [HttpPost]
        // public async Task<ActionResult> RegisterAsync([FromBody] UserAuth model)
        // {
        //     var user = await _repository.CreateUser(model);
        //     return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        // }

        // POST: api/User/Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserAuth model)
        {
            if (model.Email == null || model.Password == null)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _authService.Login(model);
            if (result.Successful)
                return Ok(result);

            return BadRequest(result);
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
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);

            return NotFound(result);
        }


        // PUT: api/User/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] User user)
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
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
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
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.RemoveById(id);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }
    }
}