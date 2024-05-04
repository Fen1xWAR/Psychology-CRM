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
            return Ok(await _repository.GetById(id));
        }
        

        // PUT: api/Psychologist/5
        [HttpPut]
        public async Task<ActionResult>Insert([FromBody]Psychologist psychologist)
        {
            await _repository.Put(psychologist);
            return Ok();
        }
        // POST: api/Psychologist
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody]Psychologist dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok();

        }

        // DELETE: api/Psychologist/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) 
        {
            await _repository.RemoveById(id);
            return Ok();
        }
    }
}
