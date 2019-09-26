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
    public class SpeakersModel : PageModel
    {
         private readonly IApiClient _apiClient;

        public SpeakersModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public ICollection<SpeakerResponse> Speakers { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Speakers = await _apiClient.GetSpeakersAsync();

            if (Speakers == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            await _apiClient.DeleteSpeakerAsync(id);

            return RedirectToPage();
        }
    }
}