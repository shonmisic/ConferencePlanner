using ConferenceDTO;
using FrontEnd.Models;
using FrontEnd.Pages.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Pages.Admin
{
    public class TracksModel : SessionsContainingPageModel<TracksModel>
    {
        public TracksModel(IApiClient apiClient, ILogger<TracksModel> logger)
            : base(apiClient, logger)
        {
        }

        public ICollection<TrackResponse> Tracks { get; set; }
        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> SessionsGroupedByTime { get; set; }
        public IEnumerable<(int Offset, DayOfWeek? DayOfWeek)> DayOffsets { get; set; }
        public int CurrentDayOffset { get; set; }
        public int SelectedTrackId { get; set; }
        public ConferenceResponse Conference { get; set; }
        public SessionsPartialModel SessionsPartialModel { get; set; }

        public async Task<IActionResult> OnGet(int conferenceId, int trackId = 0, int day = 0)
        {
            CurrentDayOffset = day;
            SelectedTrackId = trackId;

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
                SelectedConferenceID = conferenceId,
                SessionsGroupedByTime = SessionsGroupedByTime,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int trackId, int day, int conferenceId)
        {
            await _apiClient.DeleteTrackAsync(trackId);

            return RedirectToPage(new { conferenceId, day, trackId });
        }

        public bool AreThereAnySessions()
        {
            return Tracks.Any() && SelectedTrackId > 0 && !SessionsGroupedByTime.Any();
        }
    }
}