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
        private IContactRepository _contactRepository;
        private IUserRepository _userRepository;

        public ContactController(IContactRepository contactRepository, IUserRepository userRepository)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        // GET: api/Contact
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _contactRepository.GetAll());
        }
        
        // GET: api/Contact/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _contactRepository.GetById(id);
            if (result.Successful)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByUserId(Guid id)
        {
            if(id == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var client = await _userRepository.GetById(id);
            if (!client.Successful)
                return BadRequest(client);
            var result = await _contactRepository.GetById(client.Result.ContactId);
            if (result.Successful)
                return Ok(result);

            return NotFound(result);
            
        }

        // PUT api/Contact/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] ContactModel contact)

        {
            if (contact.Name == "" || contact.Lastname == ""|| contact.DateOfBirth==DateOnly.FromDateTime(DateTime.Now))
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _contactRepository.Put(contact);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Contact dataToUpdate)
        {
            if (dataToUpdate.ContactId == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _contactRepository.Update(dataToUpdate);
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
            var result = await _contactRepository.RemoveById(id);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }
    }
}