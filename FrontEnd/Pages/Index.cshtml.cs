using ConferenceDTO;
using FrontEnd.Models;
using FrontEnd.Pages.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FrontEnd.Pages
{
    public class IndexModel : SessionsContainingPageModel<IndexModel>
    {
        public IndexModel(IApiClient apiClient, ILogger<IndexModel> logger)
            : base(apiClient, logger)
        {
        }

        public IEnumerable<ConferenceResponse> Conferences { get; set; }
        public ConferenceResponse SelectedConference { get; set; }
        public bool IsAdmin { get; set; }
        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> SessionsGroupedByTime { get; set; }
        public IEnumerable<(int Offset, DayOfWeek? DayOfWeek)> DayOffsets { get; set; }
        public int CurrentDayOffset { get; set; }
        public SessionsPartialModel SessionsPartialModel { get; set; }

        [TempData]
        public string Message { get; set; }
        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGet(int conferenceId = 0, int day = 0)
        {
            _logger.LogDebug("OnGet was called");

            CurrentDayOffset = day;

            IsAdmin = User.IsAdmin();

            Conferences = await _apiClient.GetConferencesForFollowingFiveDays();

            SelectedConference = Conferences.SingleOrDefault(c => c.ID == conferenceId);

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

            SessionsGroupedByTime = sessions.Where(s => s.StartTime?.Date == filterDate)
                .OrderBy(s => s.TrackId)
                .GroupBy(s => s.StartTime)
                .OrderBy(s => s.Key);

            SessionsPartialModel = new SessionsPartialModel
            {
                AttendeeUsername = User.Identity.Name,
                SelectedConferenceID = SelectedConference.ID,
                SessionsGroupedByTime = SessionsGroupedByTime,
            };
        }

        public async Task<IActionResult> OnPostRemoveAsync(int conferenceId)
        {
            await _apiClient.DeleteConference(conferenceId);

            return RedirectToPage();
        }
    }
}
