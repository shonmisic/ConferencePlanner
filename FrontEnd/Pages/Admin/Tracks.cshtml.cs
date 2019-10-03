using ConferenceDTO;
using FrontEnd.Pages.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FrontEnd.Pages.Admin
{
    public class TracksModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public TracksModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public ICollection<TrackResponse> Tracks { get; set; }
        public ConferenceResponse Conference { get; set; }
        public SessionsPartialModel SessionsPartialModel { get; set; }

        public async Task<IActionResult> OnGet(int conferenceId, int trackId = 0, int day = 0)
        {
            Conference = await _apiClient.GetConference(conferenceId);

            Tracks = await _apiClient.GetTracks(conferenceId);

            if (Tracks == null)
            {
                return NotFound();
            }

            var sessions = await _apiClient.GetSessionsByTrackAsync(trackId);

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
                SelectedConferenceID = conferenceId,
                Sessions = sessionsGroupedByTime,
                ParentPagePath = "/Admin/Tracks",
                CurrentDayOffset = day,
                DayOffsets = Enumerable.Range(0, numberOfDays ?? 0)
                                .Select(offset => (offset, startDate?.AddDays(offset).DayOfWeek))
            };

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int trackId)
        {
            await _apiClient.DeleteTrackAsync(trackId);

            return RedirectToPage();
        }
    }
}