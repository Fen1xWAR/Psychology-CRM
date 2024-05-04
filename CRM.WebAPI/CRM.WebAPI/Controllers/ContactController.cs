using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
            if (id == Guid.Empty)
                return BadRequest("Id is empty");
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);

            return NotFound($"Contact with id {id} does not exist");
        }

        // PUT api/Contact/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] Contact contact)

        {

            var result = await _repository.Put(contact);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Contact dataToUpdate)
        {
            var result =  await _repository.Update(dataToUpdate);
            if (!result.Successful)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }

        // DELETE api/Contact/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result =  await _repository.RemoveById(id);
            if (!result.Successful)
            {
                return BadRequest(result.ErrorMessage);
            }
           
            return Ok();
        }
    }
}