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
    public class CreateConferenceModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public CreateConferenceModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public ConferenceRequest Conference { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            await _apiClient.CreateConferenceAsync(Conference);

            return RedirectToPage("/Index");
        }
    }
}