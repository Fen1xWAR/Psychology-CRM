using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        private IVisitRepository _repository;

        public VisitController(IVisitRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        // GET: api/Visit
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET api/Visit/5
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


        // PUT api/Visit/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] Visit visit)

        {
            await _repository.Put(visit);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Visit dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok();
        }

        // DELETE api/Visit/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _repository.RemoveById(id);
            return Ok();
        }
    }
}