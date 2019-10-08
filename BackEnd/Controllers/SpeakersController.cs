using BackEnd.Infrastructure;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly ISpeakersRepository _speakersRepository;

        private static readonly string _getSessions = "GetSessions";

        public SpeakersController(ISessionsRepository sessionsRepository,
            ISpeakersRepository speakersRepository)
        {
            _sessionsRepository = sessionsRepository;
            _speakersRepository = speakersRepository;
        }

        // GET: api/Speakers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConferenceDTO.SpeakerResponse>>> GetSpeakers()
        {
            return (await _speakersRepository.GetAllAsync()
                                            .ToListAsync())
                                            .Select(s => s.MapSpeakerResponse())
                                            .ToList();
        }

        // GET: api/Speakers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConferenceDTO.SpeakerResponse>> GetSpeaker(int id)
        {
            var speaker = await _speakersRepository.GetByIdAsync(id);

            if (speaker == null)
            {
                return NotFound();
            }

            return speaker.MapSpeakerResponse();
        }

        // PUT: api/Speakers/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutSpeaker(int id, ConferenceDTO.SpeakerRequest input)
        {
            try
            {
                var speaker = await _speakersRepository.UpdateAsync(input.MapSpeaker());

                if (speaker == null)
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_speakersRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Speakers
        [HttpPost]
        public async Task<ActionResult<ConferenceDTO.SpeakerResponse>> PostSpeaker(ConferenceDTO.SpeakerRequest input)
        {
            var speaker = await _speakersRepository.AddAsync(input.MapSpeaker());

            return CreatedAtAction(nameof(GetSpeaker), new { id = speaker.ID }, speaker.MapSpeakerResponse());
        }

        // DELETE: api/Speakers/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ConferenceDTO.SpeakerResponse>> DeleteSpeaker(int id)
        {
            var speaker = await _speakersRepository.DeleteAsync(id);

            if (speaker == null)
            {
                return NotFound();
            }

            foreach (var sessionId in speaker.SessionSpeakers.Select(ss => ss.SessionId))
            {
                var session = await _sessionsRepository.GetByIdAsync(sessionId);

                if (!session.SessionSpeakers.Any())
                {
                    await _sessionsRepository.DeleteAsync(sessionId);
                }
            }

            return speaker.MapSpeakerResponse();
        }
    }
}
