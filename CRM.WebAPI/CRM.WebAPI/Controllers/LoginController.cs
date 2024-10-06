using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ConflictResult = CRM.Core.Implement.ConflictResult;


namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private ILoginRepository _repository;

        public LoginController(ILoginRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/Login/5
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


        // PUT: api/Login/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] LoginModel login)
        {
            if(login.UserId == Guid.Empty || login.LoginTime == null)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Put(login);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // POST: api/Login
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody] Login dataToUpdate)
        {
            if (dataToUpdate.LoginId == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) //удаляет по id
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