using CRM.Core.Implement;
using Microsoft.AspNetCore.Mvc;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ConflictResult = CRM.Core.Implement.ConflictResult;

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]

    //Контроллер - управляет таблицей в данном случае таблицей клиентов
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private IClientRepository _repository; //храним репозиторий (что то что напрямую взаимодействует с базой)


        public
            ClientController(
                IClientRepository repository) //  конструктор где принимаем репозиторий (его передаем как DI в классе program)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        // GET: api/Client
        [HttpGet]
        public async Task<IActionResult> GetAll() // метод возвращает все
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/Client/5
        [HttpGet("{id}")] // ищет по id
        public async Task<ActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);
            return NotFound(result);
        }


        // PUT: api/Client/5
        [HttpPut] //добавляет в базу новый
        public async Task<IActionResult> Insert([FromBody] ClientModel client)

        {
            if (client.FormId == Guid.Empty || client.UserId == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Put(client);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult>
            Update([FromBody] Client dataToUpdate) //обновляет данные (что угодно можно поменять в клиенте кроме id)
        {
            if (dataToUpdate.ClientId == Guid.Empty)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.Update(dataToUpdate);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // DELETE api/Client/5
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