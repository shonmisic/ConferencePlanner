using ConferenceDTO;
using FrontEnd.Models;
using FrontEnd.Pages.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FrontEnd.Pages
{
    public class IndexModel : SessionsContainingPageModel<IndexModel>
    {
        public IndexModel(IApiClient apiClient, ILogger<IndexModel> logger) : base(apiClient, logger)
        {
        }

        public IEnumerable<ConferenceResponse> Conferences { get; set; }
        public ConferenceResponse SelectedConference { get; set; }
        public bool IsAdmin { get; set; }
        public SessionsPartialModel SessionsPartialModel { get; set; }

        [TempData]
        public string Message { get; set; }
        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public bool AreThereAnyConferences { get; set; }

        public async Task OnGet(int conferenceId = 0, int day = 0)
        {
            _logger.LogDebug("OnGet was called");

            IsAdmin = User.IsAdmin();

            Conferences = await _apiClient.GetConferencesForFollowingFiveDays();

            AreThereAnyConferences = Conferences.Any();

            SelectedConference = Conferences.SingleOrDefault(c => c.ID == conferenceId);

            if (SelectedConference == null)
            {
                return;
            }

            var sessions = await GetSessionsAsync(SelectedConference.ID);

            var startDate = sessions.Min(s => s.StartTime?.Date);
            var endDate = sessions.Max(s => s.EndTime?.Date);

            var numberOfDays = (endDate - startDate)?.Days + 1;

            var filterDate = startDate?.AddDays(day);

            var sessionsGroupedByTime = sessions.Where(s => s.StartTime?.Date == filterDate)
                .OrderBy(s => s.TrackId)
                .GroupBy(s => s.StartTime)
                .OrderBy(s => s.Key);

            SessionsPartialModel = new SessionsPartialModel
            {
                AttendeeUsername = User.Identity.Name,
                SelectedConferenceID = SelectedConference.ID,
                Sessions = sessionsGroupedByTime,
                ParentPagePath = "/Index",
                CurrentDayOffset = day,
                DayOffsets = Enumerable.Range(0, numberOfDays ?? 0)
                                .Select(offset => (offset, startDate?.AddDays(offset).DayOfWeek))
            };
        }

        public async Task<IActionResult> OnPostRemoveAsync(int conferenceId)
        {
            await _apiClient.DeleteConference(conferenceId);

            return RedirectToPage();
        }
    }
}
