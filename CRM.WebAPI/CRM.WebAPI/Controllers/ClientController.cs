using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using CRM.Infrastructure.Repositories;

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private IClientRepository _clientRepository;


        public ClientController(IClientRepository repository)
        {
            _clientRepository = repository ?? throw new ArgumentException(nameof(repository));
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _clientRepository.GetAll());
        }

        // GET: api/Client/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _clientRepository.GetById(id));
            }
            catch (Exception e)
            {
                return BadRequest(StatusCodes.Status404NotFound);
            }
        }


        // PUT: api/Client/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromBody] Client client)

        {
            await _clientRepository.Put(client);
            return Ok(StatusCodes.Status200OK);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] Client dataToUpdate)
        {
            await _clientRepository.Update(dataToUpdate);
            return Ok(StatusCodes.Status200OK);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _clientRepository.RemoveById(id);
            return Ok(StatusCodes.Status200OK);
        }
    }
}