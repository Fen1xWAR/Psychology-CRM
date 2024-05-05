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
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);

            return NotFound(result);
        }

        // PUT api/Contact/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] ContactModel contact)

        {
            if (contact.Name == "" || contact.Lastname == "")
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Put(contact);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Contact dataToUpdate)
        {
            if (dataToUpdate.ContactId == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // DELETE api/Contact/5
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