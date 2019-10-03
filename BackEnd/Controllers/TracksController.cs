using BackEnd.Infrastructure;
using BackEnd.Repositories;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ITracksRepository _tracksRepository;
        private readonly IDistributedCache _cache;

        private static readonly string _getConferences = "GetConferences";

        public TracksController(ITracksRepository tracksRepository, IDistributedCache cache)
        {
            _tracksRepository = tracksRepository;
            _cache = cache;
        }

        [HttpGet("conference/{conferenceId:int}")]
        public async Task<ActionResult<ICollection<TrackResponse>>> GetByConference(int conferenceId)
        {
            var result = (await _tracksRepository.GetAllByConferenceIdAsync(conferenceId))
                                                 .Select(t => t.MapTrackResponse())
                                                 .ToList();

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrackResponse>> GetById(int id)
        {
            var result = await _tracksRepository.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return result.MapTrackResponse();
        }

        [HttpPost]
        public async Task<ActionResult<TrackResponse>> Post(TrackRequest input)
        {
            var track = await _tracksRepository.AddAsync(input.MapTrack());

            var result = track.MapTrackResponse();

            return CreatedAtAction(nameof(GetById), new { result.ID }, result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<TrackResponse>> Delete(int id)
        {
            var track = await _tracksRepository.GetByIdAsync(id);

            if (track == null)
            {
                return NotFound();
            }

            await _tracksRepository.DeleteAsync(id);

            return track.MapTrackResponse();
        }

        private async Task CacheValue<T>(string key, T result)
        {
            var valueToCache = result.ToByteArray();
            var options = new DistributedCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(1));
            await _cache.SetAsync(key, valueToCache, options);
        }
    }
}