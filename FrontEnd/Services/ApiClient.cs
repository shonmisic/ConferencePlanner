using ConferenceDTO;
using FrontEnd.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Services
{
    public class ApiClient : IApiClient
    {
        private static readonly string _sessionsUri = "/api/sessions";
        private static readonly string _attendeesUri = "/api/attendees";
        private static readonly string _speakersUri = "/api/speakers";
        private static readonly string _searchUri = "/api/search";
        private static readonly string _imagesUri = "/api/images";

        private readonly IMemoryCache _cache;
        private static readonly string _getSessionsKey = "_GetSessions";
        private static readonly string _getSpeakersKey = "_GetSpeakers";
        private static readonly string _getSearchResults = "_GetSearchResults";
        private static readonly string _getImages = "_GetImages";

        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient, MemoryCacheSingleton memoryCacheSingleton)
        {
            _httpClient = httpClient;
            _cache = memoryCacheSingleton.Cache;
        }

        public async Task<bool> AddAttendeeAsync(Attendee attendee)
        {
            var response = await _httpClient.PostAsJsonAsync(_attendeesUri, attendee);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task AddSessionToAttendeeAsync(string name, int sessionId)
        {
            var response = await _httpClient.PostAsync($"{_attendeesUri}/{name}/session/{sessionId}", null);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteSessionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_sessionsUri}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<AttendeeResponse> GetAttendeeAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"{_attendeesUri}/{name}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<AttendeeResponse>();
        }

        public async Task<SessionResponse> GetSessionAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_sessionsUri}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<SessionResponse>();
        }

        public async Task<ICollection<SessionResponse>> GetSessionsAsync()
        {
            if (!_cache.TryGetValue(_getSessionsKey, out ICollection<SessionResponse> sessions))
            {
                var response = await _httpClient.GetAsync(_sessionsUri);

                response.EnsureSuccessStatusCode();

                sessions = await response.Content.ReadAsAsync<ICollection<SessionResponse>>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(10240)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(_getSessionsKey, sessions, cacheEntryOptions);
            }

            return sessions;
        }

        public async Task<ICollection<SessionResponse>> GetSessionsByAttendeeAsync(string name)
        {
            if (!_cache.TryGetValue($"{_getSessionsKey}/{name}", out ICollection<SessionResponse> sessions))
            {
                var attendeeTask = GetAttendeeAsync(name);
                var sessionsTask = GetSessionsAsync();

                await Task.WhenAll(attendeeTask, sessionsTask);

                var attendee = await attendeeTask;
                sessions = await sessionsTask;

                if (attendee == null)
                {
                    return new List<SessionResponse>();
                }

                var sessionIds = attendee.Sessions.Select(s => s.ID);

                sessions = sessions.Where(s => sessionIds.Contains(s.ID)).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSize(10240)
                   .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(_getSessionsKey, sessions, cacheEntryOptions);
            }

            return sessions;
        }

        public async Task<SpeakerResponse> GetSpeakerAsync(int id)
        {
            if (!_cache.TryGetValue($"{_getSpeakersKey}/{id}", out SpeakerResponse speaker))
            {
                var response = await _httpClient.GetAsync($"{_speakersUri}/{id}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                speaker = await response.Content.ReadAsAsync<SpeakerResponse>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetSize(1024)
                  .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(_getSpeakersKey, speaker, cacheEntryOptions);
            }

            return speaker;
        }

        public async Task<ICollection<SpeakerResponse>> GetSpeakersAsync()
        {
            if (!_cache.TryGetValue(_getSpeakersKey, out ICollection<SpeakerResponse> speakers))
            {
                var response = await _httpClient.GetAsync(_speakersUri);

                response.EnsureSuccessStatusCode();

                speakers = await response.Content.ReadAsAsync<ICollection<SpeakerResponse>>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetSize(10240)
                  .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(_getSpeakersKey, speakers, cacheEntryOptions);
            }

            return speakers;
        }

        public async Task PutSessionAsync(Session session)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_sessionsUri}/{session.ID}", session);

            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveSessionFromAttendeeAsync(string name, int sessionId)
        {
            var response = await _httpClient.DeleteAsync($"{_attendeesUri}/{name}/session/{sessionId}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<ICollection<SearchResult>> SearchAsync(string query)
        {
            if (!_cache.TryGetValue($"{_getSearchResults}/{query}", out ICollection<SearchResult> searchResults))
            {
                var term = new SearchTerm
                {
                    Query = query
                };

                var response = await _httpClient.PostAsJsonAsync(_searchUri, term);

                response.EnsureSuccessStatusCode();

                searchResults = await response.Content.ReadAsAsync<ICollection<SearchResult>>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetSize(10240)
                  .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(_getSearchResults, searchResults, cacheEntryOptions);
            }

            return searchResults;
        }

        public async Task<bool> CheckHealthAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("/health");

                return string.Equals(response, "Healthy", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public async Task<ICollection<ImageResponse>> GetImagesAsync()
        {
            if (!_cache.TryGetValue(_getImages, out ICollection<ImageResponse> images))
            {
                var response = await _httpClient.GetAsync(_imagesUri);

                response.EnsureSuccessStatusCode();

                images = await response.Content.ReadAsAsync<ICollection<ImageResponse>>();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetSize(102400)
                  .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(_getImages, images, cacheEntryOptions);
            }

            return images;
        }

        public async Task AddImageToAttendeeAsync(string username, ImageRequest imageRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_attendeesUri}/{username}/image", imageRequest);

            response.EnsureSuccessStatusCode();
        }
    }
}
