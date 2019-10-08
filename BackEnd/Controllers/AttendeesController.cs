using BackEnd.Infrastructure;
using BackEnd.Repositories;
using ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly IAttendeesRepository _attendeesRepository;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IConferencesRepository _conferencesRepository;

        public AttendeesController(IAttendeesRepository attendeesRepository,
            ISessionsRepository sessionsRepository,
            IConferencesRepository conferencesRepository)
        {
            _attendeesRepository = attendeesRepository;
            _sessionsRepository = sessionsRepository;
            _conferencesRepository = conferencesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<AttendeeResponse>>> GetAll()
        {
            return await _attendeesRepository.GetAll()
                                            .Select(a => a.MapAttendeeResponse())
                                            .ToListAsync();
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<AttendeeResponse>> GetByUsername(string username)
        {
            var attendee = await _attendeesRepository.GetByUsernameAsync(username);

            if (attendee == null)
            {
                return NotFound();
            }

            return attendee.MapAttendeeResponse();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AttendeeResponse>> Post(Attendee input)
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

            return CreatedAtAction(nameof(GetByUsername), new { username = result.UserName }, result);
        }

        // PUT: api/Speakers/5
        [HttpPut("{username}")]
        public async Task<IActionResult> PutAttendee(string username, Attendee input)
        {
            try
            {
                if (username != input.UserName)
                {
                    return BadRequest();
                }

                var attendee = await _attendeesRepository.GetByUsernameAsync(username);

                if (attendee == null)
                {
                    return NotFound();
                }

                attendee.UpdateValuesFrom(input);

                await _attendeesRepository.UpdateAsync(attendee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_attendeesRepository.GetByUsernameAsync(username) == null)
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

            var session = await _sessionsRepository.GetByIdAsync(sessionId);

            if (session == null)
            {
                return BadRequest();
            }

            var newAttendee = await _attendeesRepository.AddSessionAsync(username, sessionId);

            if (!newAttendee.ConferenceAttendees.Any(ca => ca.ConferenceId == session.ConferenceId))
            {
                newAttendee = await _attendeesRepository.AddConferenceAsync(username, session.ConferenceId);
            }

            var result = newAttendee.MapAttendeeResponse();
             
            return CreatedAtAction(nameof(GetByUsername), new { username = result.UserName }, result);
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

            var sessionAttendee = attendee.SessionAttendees.SingleOrDefault(sa => sa.SessionId == sessionId);

            if (sessionAttendee == null)
            {
                return BadRequest();
            }

            var isSuccess = await _attendeesRepository.RemoveSessionAsync(username, sessionId);

            var conferenceId = sessionAttendee.Session.ConferenceId;

            if (isSuccess && !DoesContainOtherSessionsFromTheSameConference(attendee, conferenceId))
            {
                await _attendeesRepository.RemoveConferenceAsync(username, conferenceId);
            }

            return NoContent();
        }

        [HttpPost("{username}/conference/{conferenceId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AttendeeResponse>> AddConference(string username, int conferenceId)
        {
            var attendee = await _attendeesRepository.GetByUsernameAsync(username);

            if (attendee == null)
            {
                return NotFound();
            }

            var conference = await _conferencesRepository.GetByIdAsync(conferenceId);

            if (conference == null)
            {
                return BadRequest();
            }

           var newAttendee = await _attendeesRepository.AddConferenceAsync(username, conferenceId);

            return newAttendee.MapAttendeeResponse();
        }

        [HttpDelete("{username}/conference/{conferenceId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RemoveConference(string username, int conferenceId)
        {
            var attendee = await _attendeesRepository.GetByUsernameAsync(username);

            if (attendee == null)
            {
                return NotFound();
            }

            var conferenceAttendee = attendee.ConferenceAttendees.SingleOrDefault(sa => sa.ConferenceId == conferenceId);

            if (conferenceAttendee == null)
            {
                return BadRequest();
            }

            var isSuccess = await _attendeesRepository.RemoveConferenceAsync(username, conferenceId);

            if (isSuccess)
            {
                foreach (var session in attendee.SessionAttendees.Select(sa => sa.Session).Where(s => s.ConferenceId == conferenceId))
                {
                    await _attendeesRepository.RemoveSessionAsync(username, session.ID);
                }
            }

            return NoContent();
        }

        [HttpPost("{username}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AttendeeResponse>> AddImage(string username, ImageRequest imageRequest)
        {
            var attendee = await _attendeesRepository.GetByUsernameAsync(username);

            if (attendee == null)
            {
                return NotFound();
            }

            attendee.AttendeeImages.Add(new Data.AttendeeImage
            {
                Attendee = attendee,
                Image = new Data.Image
                {
                    Name = imageRequest.Name,
                    Content = imageRequest.Content,
                    UploadDate = DateTimeOffset.Now,
                    ImageType = imageRequest.ImageType,
                }
            });

            await _attendeesRepository.UpdateAsync(attendee);

            var result = attendee.MapAttendeeResponse();

            return result;
        }

        private static bool DoesContainOtherSessionsFromTheSameConference(Data.Attendee attendee, int conferenceId)
        {
            return attendee.SessionAttendees.Select(sa => sa.Session).Any(s => s.ConferenceId == conferenceId);
        }
    }
}
