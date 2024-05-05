using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using CRM.WebAPI.ModelsToUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class VisitController : ControllerBase
    {
        private IVisitRepository _repository;
   
       
        public VisitController(IVisitRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }
        
        //Logic
        // [HttpPost]
        // public async Task<ActionResult> CreateVisit([FromForm] VisitModel visit)
        // {
        //     try
        //     {
        //
        //        await _repository.Put(new Visit()
        //         {
        //             VisitId = new Guid(),
        //             ClientId = visit.ClientId,
        //             ClientNote = visit.ClientNote,
        //             DateTime = visit.DateTime,
        //             PsychologistDescription = visit.Description,
        //             PsychologistId = visit.PsychologistId,
        //             ServiceId = visit.ServiceId
        //         });
        //         return Ok();
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }
        
        
        
        
        
        
        
        
        
        //Cruds
        
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
            if (id == Guid.Empty)
                return BadRequest("Id is empty");
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);
            return NotFound(result.ErrorMessage);
        }


        // PUT api/Visit/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] Visit visit)

        {
            var result = await _repository.Put(visit);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Visit dataToUpdate)
        {
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        // DELETE api/Visit/5
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