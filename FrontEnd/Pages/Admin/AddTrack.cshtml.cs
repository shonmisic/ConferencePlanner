using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Admin
{
    public class AddTrackModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public AddTrackModel(IApiClient apiClient)
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