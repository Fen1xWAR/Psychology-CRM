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
    public class FormController : ControllerBase
    {
        private IFormRepository _repository;

        public FormController(IFormRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        // GET: api/Form
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET api/Form/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                return Ok(await _repository.GetById(id));
            }
            catch (Exception e)
            {
                return BadRequest(StatusCodes.Status404NotFound);
            }
        }


        // PUT api/Form/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] Form visit)

        {
            await _repository.Put(visit);
            return Ok(StatusCodes.Status200OK);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Form dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok(StatusCodes.Status200OK);
        }

        // DELETE api/Form/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _repository.RemoveById(id);
            return Ok(StatusCodes.Status200OK);
        }
    
    }
}
