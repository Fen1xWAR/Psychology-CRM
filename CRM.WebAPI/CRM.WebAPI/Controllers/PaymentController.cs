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
    public class PaymentController : ControllerBase
    {
        private IPaymentRepository _repository;

        public PaymentController(IPaymentRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }
        // GET: api/Payment
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            return Ok(await _repository.GetById(id));
        }
        

        // PUT: api/Payment/5
        [HttpPut]
        public async Task<ActionResult>Insert([FromBody]Payment payment)
        {
            await _repository.Put(payment);
            return Ok();
        }
        // POST: api/Payment
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody]Payment dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok();

        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) //удаляет по id
        {
            await _repository.RemoveById(id);
            return Ok();
        }
    }
}
