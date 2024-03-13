using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using CRM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        private IVisitRepository _visitRepository;

        public VisitController(IVisitRepository visitRepository)
        {
            _visitRepository = visitRepository ?? throw new ArgumentException(nameof(visitRepository));
        }

        // GET: api/values
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _visitRepository.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetByID")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                return Ok(await _visitRepository.GetById(id));
            }
            catch (Exception e)
            {
                return BadRequest(StatusCodes.Status404NotFound);
            }
        }


        // PUT api/values/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] Visit visit)

        {
            await _visitRepository.Put(visit);
            return Ok(StatusCodes.Status200OK);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Visit dataToUpdate)
        {
            await _visitRepository.Update(dataToUpdate);
            return Ok(StatusCodes.Status200OK);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _visitRepository.RemoveById(id);
            return Ok(StatusCodes.Status200OK);
        }
    }
}