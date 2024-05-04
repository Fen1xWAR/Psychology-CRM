using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using CRM.WebAPI.ModelsToUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using File = CRM.Domain.Models.File;

namespace CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private IFileRepository _repository;

        public FileController(IFileRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/File
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/File/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id is empty");
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);
            return NotFound($"File with id {id} does not exist");
        }

        // PUT api/File/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromForm] FileToUpload file)

        {
            if (file.ClientId == Guid.Empty || file.PsychologistId == Guid.Empty)
            {
                return BadRequest("Empty value does not allowed");
            }

            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await file.Files.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }


            var fileToPut = new File()
            {
                FileId = Guid.NewGuid(),
                ClientId = file.ClientId,
                PsychologistId = file.PsychologistId,
                FileName = file.Files.FileName,
                FileContent = fileBytes,
            };
            var result = await _repository.Put(fileToPut);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromForm] Guid fileId, [FromForm] FileToUpload dataToUpdate)
        {
            if (fileId == Guid.Empty)
                return BadRequest("File id is empty!");


            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await dataToUpdate.Files.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }

            var fileToUpdate = new File()
            {
                FileId = fileId,
                ClientId = dataToUpdate.ClientId,
                PsychologistId = dataToUpdate.PsychologistId,
                FileName = dataToUpdate.Files.FileName,
                FileContent = fileBytes,
            };
            var result = await _repository.Update(fileToUpdate);
            if (!result.Successful)
                return BadRequest(result.ErrorMessage);
            return Ok();
        }

        // DELETE api/File/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("File id is empty!");
            var result = await _repository.RemoveById(id);
            if (!result.Successful)
                return BadRequest(result.ErrorMessage);
            return Ok();
        }
    }
}