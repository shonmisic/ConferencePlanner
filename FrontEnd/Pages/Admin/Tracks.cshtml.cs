using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
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

        public async Task<IActionResult> OnGet(int conferenceId)
        {
            Conference = await _apiClient.GetConference(conferenceId);

            Tracks = await _apiClient.GetTracks(conferenceId);

            if (Tracks == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            await _apiClient.DeleteTrackAsync(id);

            return RedirectToPage();
        }
    }
}