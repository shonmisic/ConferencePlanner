using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Infrastructure;
using BackEnd.Repositories;
using ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ITracksRepository _tracksRepository;
        private readonly IDistributedCache _cache;

        private static readonly string _getTracks = "GetTracks";

        public TracksController(ITracksRepository sessionsRepository, IDistributedCache cache)
        {
            _tracksRepository = sessionsRepository;
            _cache = cache;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrackResponse>> Get(int conferenceId)
        {
            var cacheKey = $"{_getTracks}/{conferenceId}";
            var cachedValue = await _cache.GetAsync(cacheKey);

            var result = cachedValue.FromByteArray<TrackResponse>();
            if (result == null)
            {
                var tracks = (await _tracksRepository.GetByConferenceIdAsync(conferenceId))
                                                    .Select(t => t.MapTrackResponse())
                                                    .ToList();

                if (tracks == null)
                {
                    return NotFound();
                }

                await CacheValue(cacheKey, result);
            }

            return result;
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