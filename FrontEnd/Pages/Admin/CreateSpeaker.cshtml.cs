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
    public class CreateSpeakerModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public CreateSpeakerModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public SpeakerRequest Speaker { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            await _apiClient.CreateSpeakerAsync(Speaker);

            return RedirectToPage("/Admin/Speakers");
        }
    }
}