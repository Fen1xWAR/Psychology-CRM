using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            if (id == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);

            return NotFound(result);
        }


        // PUT: api/Payment/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] PaymentModel payment)
        {
            if (payment.ClientId == Guid.Empty || payment.PaymentAmount == null || payment.PaymentDate == null ||
                payment.PaymentMethod == null)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Put(payment);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // POST: api/Payment
        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody] Payment dataToUpdate)
        {
            if (dataToUpdate.PaymentId == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) //удаляет по id
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