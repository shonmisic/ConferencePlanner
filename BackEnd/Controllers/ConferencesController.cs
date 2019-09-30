using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Infrastructure;
using ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferencesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ConferencesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<ConferenceResponse>>> GetAllConferences()
        {
            return await _db.Conferences.AsNoTracking()
                                        .Select(s => s.MapConferenceResponse())
                                        .ToListAsync();
        }

        [HttpGet("/5-days")]
        public async Task<ActionResult<List<ConferenceResponse>>> GetConferencesForFollowingFiveDays()
        {
            var dateTimeNow = DateTimeOffset.Now;

            return await _db.Conferences.AsNoTracking()
                                        .Where(c => IsConferenceWithinDateRange(dateTimeNow, dateTimeNow.AddDays(5), c))
                                        .Select(s => s.MapConferenceResponse())
                                        .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ConferenceResponse>> GetConference(int id)
        {
            var conference = await _db.FindAsync<Data.Conference>(id);

            if (conference == null)
            {
                return NotFound();
            }

            var result = new ConferenceResponse
            {
                ID = conference.ID,
                Name = conference.Name,
                StartTime = conference.StartTime,
                EndTime = conference.EndTime,
                Url = conference.Url
            };

            return result;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadConference([Required, FromForm]string conferenceName, [FromForm]ConferenceFormat format, IFormFile file)
        {
            var loader = GetLoader(format);

            using (var stream = file.OpenReadStream())
            {
                await loader.LoadDataAsync(conferenceName, stream, _db);
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ConferenceResponse>> CreateConference(ConferenceDTO.Conference input)
        {
            var conference = new Data.Conference
            {
                Name = input.Name
            };

            _db.Conferences.Add(conference);
            await _db.SaveChangesAsync();

            var result = new ConferenceResponse
            {
                ID = conference.ID,
                Name = conference.Name
            };

            return CreatedAtAction(nameof(GetConference), new { id = conference.ID }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutConference(int id, ConferenceDTO.Conference input)
        {
            var conference = await _db.FindAsync<Data.Conference>(id);

            if (conference == null)
            {
                return NotFound();
            }

            conference.Name = input.Name;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ConferenceResponse>> DeleteConference(int id)
        {
            var conference = await _db.FindAsync<Data.Conference>(id);

            if (conference == null)
            {
                return NotFound();
            }

            _db.Remove(conference);

            await _db.SaveChangesAsync();

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
