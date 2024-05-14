using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Core.Implement;
using CRM.Domain.Models;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebAPI
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CurrentUserController : ControllerBase
    {
        private IAuthService _authService;
        private IContactRepository _contactRepository;
        private IUserRepository _userRepository;
        private IVisitRepository _visitRepository;

        public CurrentUserController(IAuthService authService, IContactRepository contactRepository,
            IUserRepository userRepository, IVisitRepository visitRepository)
        {
            _authService = authService;
            _contactRepository = contactRepository;
            _userRepository = userRepository;
            _visitRepository = visitRepository;
        }

        [HttpGet]
        public ActionResult GetCurrentUserData()
        {
            var result = _authService.GetCurrentUser(HttpContext);
            if (result.Successful)
                return Ok(result);
            return Unauthorized(result);
        }

        // GET: api/CurrentUser
        [HttpGet]
        public async Task<ActionResult> GetContact()
        {
            var currentUser = await _authService.GetCurrentUserIternal(HttpContext);
            if (!currentUser.Successful)
                return BadRequest(currentUser);
            var contact = await _contactRepository.GetById(currentUser.Result.ContactId);
            if (!contact.Successful)
                return BadRequest(contact);
            return Ok(contact);
        }

        [HttpGet]
        public async Task<IActionResult> GetVisits()
        {
            var currentUser = await _authService.GetCurrentUserIternal(HttpContext);
            if (!currentUser.Successful)
                return BadRequest(currentUser);
            if (currentUser.Result.Role == "Client")
            {
                return Ok(_visitRepository.GetAllByClientId(currentUser.Result.UserId));
            }
            else if (currentUser.Result.Role == "Psychologist")
            {
                return Ok(_visitRepository.GetAllByPsychologistId(currentUser.Result.UserId));
            }

            throw new Exception("Invalid role to find visits!");
        }

        // GET: api/CurrentUser/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CurrentUser
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/CurrentUser/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/CurrentUser/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}