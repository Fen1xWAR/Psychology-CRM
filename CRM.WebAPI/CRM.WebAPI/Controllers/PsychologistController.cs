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
{[Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PsychologistController : ControllerBase
    {
        private IPsychologistRepository _repository;

        public PsychologistController(IPsychologistRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }
        // GET: api/Psychologist
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/Psychologist/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is empty");
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);
            return NotFound(result.ErrorMessage);
        }
        

        // PUT: api/Psychologist/5
        [HttpPut]
        public async Task<ActionResult>Insert([FromBody]Psychologist psychologist)
        {
            var result = await _repository.Put(psychologist);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }
        // POST: api/Psychologist
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody]Psychologist dataToUpdate)
        {
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);

        }

        // DELETE: api/Psychologist/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) 
        {
            var result = await _repository.RemoveById(id);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }
    }
}
