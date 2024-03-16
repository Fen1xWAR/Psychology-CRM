using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using File = CRM.Domain.Models.File;

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IFileRepository _repository;

        public FileController(IFileRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        // GET: api/Contact
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/Contact/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            return Ok(await _repository.GetById(id));
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromForm] FileToUpload file)

        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.files.CopyTo(ms);
                fileBytes = ms.ToArray();
                // string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
            }
            

            var fileToPut = new File()
            {
                FileId = Guid.NewGuid(),
                ClientId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                PsychologistId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                FileName = file.files.FileName,
                FileContent = fileBytes,
            };
            await _repository.Put(fileToPut);
            return Ok(StatusCodes.Status200OK);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] File dataToUpdate)
        {
            await _repository.Update(dataToUpdate);
            return Ok(StatusCodes.Status200OK);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _repository.RemoveById(id);
            return Ok(StatusCodes.Status200OK);
        }
    }
}
