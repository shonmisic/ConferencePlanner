using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrontEnd.Pages.Admin
{
    public class AttendeesModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public AttendeesModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public ICollection<AttendeeResponse> Attendees { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Attendees = await _apiClient.GetAllAttendeesAsync();

            if (Attendees == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(string username)
        {
            await _apiClient.DeleteAttendeeAsync(username);

            return RedirectToPage();
        }
    }
}