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
            if (id == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);

            return NotFound(result);
        }


        // PUT api/Form/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] FormModel form)

        {
            var result = await _repository.Put(form);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Form dataToUpdate)
        {
            if (dataToUpdate.FormId == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // DELETE api/Form/5
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