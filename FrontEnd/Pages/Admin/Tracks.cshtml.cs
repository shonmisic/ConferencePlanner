using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; }
        public ConferenceResponse Conference { get; set; }

        public async Task<IActionResult> OnGet(int conferenceId, int id = 0)
        {
            Conference = await _apiClient.GetConference(conferenceId);

            Tracks = await _apiClient.GetTracks(conferenceId);

            if (Tracks == null)
            {
                return NotFound();
            }

            var sessions = await _apiClient.GetSessionsByTrackAsync(id);

            Sessions = sessions.OrderBy(s => s.TrackId)
                                .GroupBy(s => s.StartTime)
                                .OrderBy(s => s.Key);

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            await _apiClient.DeleteTrackAsync(id);

            return RedirectToPage();
        }
    }
}