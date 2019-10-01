using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        protected readonly IApiClient _apiClient;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IApiClient apiClient, ILogger<IndexModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public IEnumerable<ConferenceResponse> Conferences {get;set;}
        public ConferenceResponse SelectedConference { get; set; }
        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; }
        public IEnumerable<(int Offset, DayOfWeek? DayOfWeek)> DayOffsets { get; set; }
        public int CurrentDayOffset { get; set; }
        public bool IsAdmin { get; set; }

        [TempData]
        public string Message { get; set; }
        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public bool AreThereAnyConferences {get;set;}

        public async Task OnGet(int id = 0, int day = 0)
        {
            _logger.LogDebug("OnGet was called");

            IsAdmin = User.IsAdmin();

            CurrentDayOffset = day;

            Conferences = await _apiClient.GetConferencesForFollowingFiveDays();

            AreThereAnyConferences = Conferences.Any();

            SelectedConference = Conferences.SingleOrDefault(c => c.ID == id);

            if (SelectedConference == null)
            {
                return;
            }

            var sessions = await GetSessionsAsync(SelectedConference.ID);

            var startDate = sessions.Min(s => s.StartTime?.Date);
            var endDate = sessions.Max(s => s.EndTime?.Date);

            var numberOfDays = (endDate - startDate)?.Days + 1;

            DayOffsets = Enumerable.Range(0, numberOfDays ?? 0)
                .Select(offset => (offset, startDate?.AddDays(offset).DayOfWeek));

            var filterDate = startDate?.AddDays(day);

            Sessions = sessions.Where(s => s.StartTime?.Date == filterDate)
                .OrderBy(s => s.TrackId)
                .GroupBy(s => s.StartTime)
                .OrderBy(s => s.Key);
        }

        public async Task<IActionResult> OnPostRemoveAsync(int conferenceId)
        {
            await _apiClient.DeleteConference(conferenceId);

            return RedirectToPage();
        }

        protected virtual Task<ICollection<SessionResponse>> GetSessionsAsync(int conferenceId)
        {
            _logger.LogDebug("GetSessionsAsync was called");

            return _apiClient.GetSessionsAsync(conferenceId);
        }
    }
}
