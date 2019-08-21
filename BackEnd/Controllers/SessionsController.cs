using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Infrastructure;
using BackEnd.Repositories;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : Controller
    {
        private readonly ISessionsRepository _sessionsRepository;

        public SessionsController(ISessionsRepository sessionsRepository)
        {
            _sessionsRepository = sessionsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SessionResponse>>> Get()
        {
            var sessions = (await _sessionsRepository.GetAllAsync())
                                             .Select(m => m.MapSessionResponse())
                                             .ToList();
            return sessions;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SessionResponse>> Get(int id)
        {
            var session = await _sessionsRepository.GetAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            return session.MapSessionResponse();
        }

        [HttpPost]
        public async Task<ActionResult<SessionResponse>> Post(ConferenceDTO.Session input)
        {
            var session = await _sessionsRepository.AddAsync(new Data.Session
            {
                Title = input.Title,
                ConferenceId = input.ConferenceId,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                Abstract = input.Abstract,
                TrackId = input.TrackId
            });

            var result = session.MapSessionResponse();

            return CreatedAtAction(nameof(Get), new { id = result.ID }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, ConferenceDTO.Session input)
        {
            var session = await _sessionsRepository.GetAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            await _sessionsRepository.UpdateAsync(input);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<SessionResponse>> Delete(int id)
        {
            var session = await _sessionsRepository.GetAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            await _sessionsRepository.DeleteAsync(id);

            return session.MapSessionResponse();
        }
    }
}
