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
    public class SessionsController : Controller
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IDistributedCache _cache;

        private static readonly string _getSessions = "GetSessions";

        public SessionsController(ISessionsRepository sessionsRepository, IDistributedCache cache)
        {
            _sessionsRepository = sessionsRepository;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<SessionResponse>>> Get(DateTimeOffset? fromDate = null, 
            DateTimeOffset? toDate = null)
        {
            fromDate = fromDate ?? DateTimeOffset.MinValue;
            toDate = toDate ?? DateTimeOffset.MaxValue;

            var cachedValue = await _cache.GetAsync(string.Format("{0}, from:{1}, to:{2}", _getSessions, fromDate, toDate));

            var result = cachedValue.FromByteArray<List<SessionResponse>>();
            
            if (result == null)
            {
                result = (await _sessionsRepository.GetAllAsync())
                            .Where(s => IsWithinDateRange(fromDate, toDate, s))
                            .Select(m => m.MapSessionResponse())
                            .ToList();

                await CacheValue(_getSessions, result);
            }

            return result;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SessionResponse>> Get(int id)
        {
            var cacheKey = $"{_getSessions}/{id}";
            var cachedValue = await _cache.GetAsync(cacheKey);

            var result = cachedValue.FromByteArray<SessionResponse>();
            if (result == null)
            {
                var session = await _sessionsRepository.GetAsync(id);

                if (session == null)
                {
                    return NotFound();
                }

                result = session.MapSessionResponse();

                await CacheValue(cacheKey, result);
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<SessionResponse>> Post(Session input)
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
        public async Task<IActionResult> Put(int id, Session input)
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

        private async Task CacheValue<T>(string key, T result)
        {
            var valueToCache = result.ToByteArray();
            var options = new DistributedCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(1));
            await _cache.SetAsync(key, valueToCache, options);
        }

        private static bool IsWithinDateRange(DateTimeOffset? fromDate, DateTimeOffset? toDate, Data.Session s)
        {
            var startTime = s.StartTime ?? DateTimeOffset.MinValue;
            var endTime = s.EndTime ?? DateTimeOffset.MaxValue;

            return startTime.CompareTo(fromDate.Value) >= 0 
                && endTime.CompareTo(toDate.Value) <= 0;
        }
    }
}
