using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FrontEnd.Pages.Admin
{
    public class CreateTrackModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public CreateTrackModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public TrackRequest Track { get; set; }
        public int ConferenceId { get; set; }

        public void OnGet(int conferenceId)
        {
            ConferenceId = conferenceId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _apiClient.CreateTrackAsync(Track);

            return RedirectToPage("/Admin/Tracks", new { Track.ConferenceId });
        }
    }
}