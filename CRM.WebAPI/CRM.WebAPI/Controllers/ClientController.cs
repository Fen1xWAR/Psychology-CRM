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
    //Контроллер - управляет таблицей в данном случае таблицей клиентов
    [ApiController]
    public class ClientController : ControllerBase
    {
        private IClientRepository _repository; //храним репозиторий (что то что напрямую взаимодействует с базой)


        public ClientController(IClientRepository repository) //  конструктор где принимаем репозиторий (его передаем как DI в классе program)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult> GetAll() // метод возвращает все
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/Client/5
        [HttpGet("{id}", Name = "GetByID")] // ищет по id
        public async Task<ActionResult> GetById(Guid id)
        {
            return Ok(await _repository.GetById(id));
        }


        // PUT: api/Client/5
        [HttpPut] //добавляет в базу новый
        public async Task<ActionResult> Insert([FromBody] Client client)

        {
            await _repository.Put(client);
            return Ok(StatusCodes.Status200OK);
        }

        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody] Client dataToUpdate) //обновляет данные (что угодно можно поменять в клиенте кроме id)
        {
            await _repository.Update(dataToUpdate);
            return Ok(StatusCodes.Status200OK);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) //удаляет по id
        {
            await _repository.RemoveById(id);
            return Ok(StatusCodes.Status200OK);
        }
    }
}