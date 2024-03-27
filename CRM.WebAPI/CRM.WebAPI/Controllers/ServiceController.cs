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
    public class ServiceController : ControllerBase
    {
        private IServiceRepository _repository;

        public ServiceController(IServiceRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }
        // GET: api/Service
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/Service/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            return Ok(await _repository.GetById(id));
        }
        

        // PUT: api/Service/5
        [HttpPut]
        public async Task<ActionResult>Insert([FromBody]Service service)
        {
            await _repository.Put(service);
            return Ok();
        }
        // POST: api/Service
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody]Service dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok();

        }

        // DELETE: api/Service/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) //удаляет по id
        {
            await _repository.RemoveById(id);
            return Ok();
        }
    }
}
