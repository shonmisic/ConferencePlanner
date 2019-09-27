using ConferenceDTO;
using FrontEnd.Infrastructure;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Admin
{
    public class CreateSessionModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public CreateSessionModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Session Speaker { get; set; }

        public IActionResult OnPostAsync()
        {
            TempData.Set(TempDataKey.NewSession, Speaker);

            return RedirectToPage("/Admin/CreateSpeaker");
        }
    }
}