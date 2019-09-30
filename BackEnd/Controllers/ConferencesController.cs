using BackEnd.Data;
using BackEnd.Infrastructure;
using BackEnd.Repositories;
using ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferencesController : ControllerBase
    {
        private readonly IConferencesRepository _conferencesRepository;

        public ConferencesController(IConferencesRepository conferencesRepository)
        {
            _conferencesRepository = conferencesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ConferenceResponse>>> GetAllConferences()
        {
            return await _conferencesRepository.GetAll()
                                               .Select(c => c.MapConferenceResponse())
                                               .ToListAsync();
        }

        [HttpGet("/5-days")]
        public async Task<ActionResult<List<ConferenceResponse>>> GetConferencesForFollowingFiveDays()
        {
            var dateTimeNow = DateTimeOffset.Now;

            return await _conferencesRepository
                            .GetAll()
                            .Where(c => IsConferenceWithinDateRange(dateTimeNow, dateTimeNow.AddDays(5), c))
                            .Select(s => s.MapConferenceResponse())
                            .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ConferenceResponse>> GetConference(int id)
        {
            var conference = await _conferencesRepository.GetByIdAsync(id);

            if (conference == null)
            {
                return NotFound();
            }

            return conference.MapConferenceResponse();
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadConference([Required, FromForm] string conferenceName, [FromForm] ConferenceFormat format, IFormFile file)
        {
            var loader = GetLoader(format);

            using (var stream = file.OpenReadStream())
            {
                var conference = await loader.LoadDataAsync(conferenceName, stream);

                await _conferencesRepository.AddAsync(conference);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ConferenceResponse>> CreateConference(ConferenceDTO.Conference input)
        {
            var conference = await _conferencesRepository.AddAsync(new Data.Conference
            {
                Name = input.Name
            });

            return CreatedAtAction(nameof(GetConference),
                new { id = conference.ID }, conference.MapConferenceResponse());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutConference(int id, ConferenceRequest input)
        {
            var conference = await _conferencesRepository.UpdateAsync(input.MapConference());

            if (conference == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ConferenceResponse>> DeleteConference(int id)
        {
            var conference = await _conferencesRepository.DeleteByIdAsync(id);

            return conference.MapConferenceResponse();
        }

        private static DataLoader GetLoader(ConferenceFormat format)
        {
            if (format == ConferenceFormat.Sessionize)
            {
                return new SessionizeLoader();
            }

            return new DevIntersectionLoader();
        }

        private static bool IsConferenceWithinDateRange(DateTimeOffset? fromDate, DateTimeOffset? toDate, Data.Conference s)
        {
            var startTime = s.StartTime ?? DateTimeOffset.MinValue;
            var endTime = s.EndTime ?? DateTimeOffset.MaxValue;

            return startTime.CompareTo(fromDate.Value) >= 0
                && endTime.CompareTo(toDate.Value) <= 0;
        }

        public enum ConferenceFormat
        {
            Sessionize,
            DevIntersections
        }
    }
}
