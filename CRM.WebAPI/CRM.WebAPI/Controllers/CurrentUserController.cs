using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebAPI.Controllers
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
        private IClientRepository _clientRepository;
        private IPsychologistRepository _psychologistRepository;

        public CurrentUserController(IAuthService authService, IContactRepository contactRepository,
            IUserRepository userRepository, IVisitRepository visitRepository, IClientRepository clientRepository,
            IPsychologistRepository psychologistRepository)
        {
            _authService = authService ?? throw new ArgumentException(nameof(authService));
            _contactRepository = contactRepository ?? throw new ArgumentException(nameof(contactRepository));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(userRepository));
            _visitRepository = visitRepository ?? throw new ArgumentException(nameof(visitRepository));
            _clientRepository = clientRepository ?? throw new ArgumentException(nameof(clientRepository));
            _psychologistRepository = psychologistRepository ?? throw new ArgumentException(nameof(psychologistRepository));
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
        public async Task<ActionResult> GetVisits()
        {
            var currentUser = await _authService.GetCurrentUserIternal(HttpContext);
            if (!currentUser.Successful)
                return BadRequest(currentUser);

            switch (currentUser.Result.Role)
            {
                case "Client":
                {
                    var client = await _clientRepository.GetByUserId(currentUser.Result.UserId);
                    if (!client.Successful)
                        return BadRequest(client);
                    var clientId = client.Result.ClientId;
                    return Ok(await _visitRepository.GetAllByClientId(clientId));
                }
                case "Psychologist":
                    var psychologist = await _psychologistRepository.GetByUserId(currentUser.Result.UserId);
                    if (!psychologist.Successful)
                        return BadRequest(psychologist);
                    var psychologistId = psychologist.Result.PsychologistId;
                    return Ok(await _visitRepository.GetAllByPsychologistId(psychologistId));
                default:
                    throw new Exception("Invalid role to find visits!");
            }
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