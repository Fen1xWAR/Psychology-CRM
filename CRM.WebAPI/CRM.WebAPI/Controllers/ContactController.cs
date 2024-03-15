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
    public class ContactController : ControllerBase
    {
        private IContactRepository _repository;

        public ContactController(IContactRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        // GET: api/Contact
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
          return Ok(await _repository.GetAll());
        }

        // GET: api/Contact/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            return Ok(await _repository.GetById(id));
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] Contact contact)

        {
            await _repository.Put(contact);
            return Ok(StatusCodes.Status200OK);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Contact dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok(StatusCodes.Status200OK);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _repository.RemoveById(id);
            return Ok(StatusCodes.Status200OK);
        }
    }
}
