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
    public class ScheduleController : ControllerBase
    {
        private IScheduleRepository _repository;

        public ScheduleController(IScheduleRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }
        // GET: api/Schedule
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/Schedule/5
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
        

        // PUT: api/Schedule/5
        [HttpPut]
        public async Task<ActionResult>Insert([FromBody]Schedule schedule)
        {
            var result = await _repository.Put(schedule);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }
        // POST: api/Schedule
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody]Schedule dataToUpdate)
        {
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);

        }

        // DELETE: api/Schedule/5
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