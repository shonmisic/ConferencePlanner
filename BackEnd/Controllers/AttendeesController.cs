using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ConferenceDTO;
using BackEnd.Infrastructure;
using BackEnd.Repositories;

namespace BackEnd
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly IAttendeesRepository _attendeesRepository;
        private readonly ISessionsRepository _sessionsRepository;

        public AttendeesController(IAttendeesRepository attendeesRepository, ISessionsRepository sessionsRepository)
        {
            _attendeesRepository = attendeesRepository;
            _sessionsRepository = sessionsRepository;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<AttendeeResponse>> Get(string username)
        {
            var attendee = await _attendeesRepository.GetByUsernameAsync(username);

            if (attendee == null)
            {
                return NotFound();
            }

            var result = attendee.MapAttendeeResponse();

            return result;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AttendeeResponse>> Post(ConferenceDTO.Attendee input)
        {
            // Check if the attendee already exists
            var existingAttendee = await _attendeesRepository.GetByUsernameAsync(input.UserName);

            if (existingAttendee != null)
            {
                return Conflict(input);
            }

            var attendee = await _attendeesRepository.AddAsync(new Data.Attendee
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.UserName,
                EmailAddress = input.EmailAddress
            });

            var result = attendee.MapAttendeeResponse();

            return CreatedAtAction(nameof(Get), new { id = result.UserName }, result);
        }

        [HttpPost("{username}/session/{sessionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AttendeeResponse>> AddSession(string username, int sessionId)
        {
            var attendee = await _attendeesRepository.GetByUsernameAsync(username);

            if (attendee == null)
            {
                return NotFound();
            }

            var session = await _sessionsRepository.GetAsync(sessionId);

            if (session == null)
            {
                return BadRequest();
            }

            var newAttendee = await _attendeesRepository.AddSessionAsync(username, sessionId);

            var result = newAttendee.MapAttendeeResponse();

            return result;
        }

        [HttpDelete("{username}/session/{sessionId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RemoveSession(string username, int sessionId)
        {
            var attendee = await _attendeesRepository.GetByUsernameAsync(username);

            if (attendee == null)
            {
                return NotFound();
            }

            var session = await _sessionsRepository.GetAsync(sessionId);

            if (session == null)
            {
                return BadRequest();
            }

            await _attendeesRepository.RemoveSessionAsync(username, sessionId);

            return NoContent();
        }
    }
}
