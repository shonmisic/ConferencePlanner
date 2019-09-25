using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{
    [Authorize]
    public class MyAgendaModel : PageModel
    {
        protected readonly IApiClient _apiClient;
        private readonly ILogger<MyAgendaModel> _logger;

        public MyAgendaModel(IApiClient apiClient, ILogger<MyAgendaModel> logger) 
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public IEnumerable<ConferenceResponse> Conferences { get; set; }
        public ConferenceResponse SelectedConference { get; set; }
        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> UserSessions { get; set; }
        public IEnumerable<(int Offset, DayOfWeek? DayOfWeek)> DayOffsets { get; set; }
        public int CurrentDayOffset { get; set; }

        [TempData]
        public string Message { get; set; }
        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGet(int day = 0, int id = 0)
        {
            CurrentDayOffset = day; Conferences = await _apiClient.GetConferencesForFollowingFiveDays();

            SelectedConference = Conferences.SingleOrDefault(c => c.ID == id);

            if (SelectedConference == null)
            {
                return;
            }

            var userSessions = await _apiClient.GetSessionsByAttendeeAsync(User.Identity.Name, id);

            var startDate = userSessions.Min(s => s.StartTime?.Date);
            var endDate = userSessions.Max(s => s.EndTime?.Date);

            var numberOfDays = (endDate - startDate)?.Days + 1;

            DayOffsets = Enumerable.Range(0, numberOfDays ?? 0)
                .Select(offset => (offset, startDate?.AddDays(offset).DayOfWeek));

            var filterDate = startDate?.AddDays(day);

            UserSessions = userSessions.Where(s => s.StartTime?.Date == filterDate)
                .OrderBy(s => s.TrackId)
                .GroupBy(s => s.StartTime)
                .OrderBy(s => s.Key);
        }

        public async Task<IActionResult> OnPostAsync(int sessionId)
        {
            await _apiClient.AddSessionToAttendeeAsync(User.Identity.Name, sessionId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int sessionId)
        {
            await _apiClient.RemoveSessionFromAttendeeAsync(User.Identity.Name, sessionId);

            return RedirectToPage();
        }
    }
}