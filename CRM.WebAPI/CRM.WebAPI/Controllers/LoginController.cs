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
            return Ok(await _repository.GetById(id));
        }
        

        // PUT: api/Login/5
        [HttpPut]
        public async Task<ActionResult>Insert([FromBody]Login login)
        {
            await _repository.Put(login);
            return Ok();
        }
        // POST: api/Login
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody]Login dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok();

        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) //удаляет по id
        {
            await _repository.RemoveById(id);
            return Ok();
        }
    }
}
