using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CRM.Domain.ModelsToUpload;
using Microsoft.AspNetCore.Authorization;
using File = CRM.Domain.Models.File;
using ConflictResult = CRM.Core.Implement.ConflictResult;

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
                return BadRequest(new ConflictResult("Empty input is not allowed!"));
            var result = await _repository.GetById(id);
            if (result.Successful)
                return Ok(result);
            return NotFound(result);
        }

        // PUT api/File/5
        [HttpPut]
        public async Task<ActionResult> Insert([FromForm] FileModel file)

        {
            if (file.ClientId == Guid.Empty || file.PsychologistId == Guid.Empty || file.Files == null)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));

            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await file.Files?.CopyToAsync(ms)!;
                fileBytes = ms.ToArray();
            }


            var fileToPut = new File()
            {
                FileId = Guid.Empty,
                ClientId = file.ClientId,
                PsychologistId = file.PsychologistId,
                FileName = file.Files.FileName,
                FileContent = fileBytes,
            };
            var result = await _repository.Put(fileToPut);
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromForm] Guid fileId, [FromForm] FileModel dataToUpdate)
        {
            if (fileId == Guid.Empty || dataToUpdate.Files == null)
                return BadRequest(new ConflictResult("Empty input is not allowed!"));


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
            if (result.Successful)
                return Ok(result);
            return BadRequest(result);
        }

        // DELETE api/File/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
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