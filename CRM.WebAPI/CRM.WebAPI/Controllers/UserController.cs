using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
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
            return Ok(await _repository.GetById(id));
        }
        

        // PUT: api/User/5
        [HttpPut]
        public async Task<ActionResult>Insert([FromBody]User user)
        {
            await _repository.Put(user);
            return Ok();
        }
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody]User dataToUpdate)
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
