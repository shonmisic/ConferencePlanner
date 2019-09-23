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
    public class TracksModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public TracksModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public ICollection<TrackResponse> Tracks { get; set; }

        public async Task<IActionResult> OnGet(int conferenceId)
        {
            Tracks = await _apiClient.GetTracks(conferenceId);

            if (Tracks == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}