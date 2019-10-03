using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<AttendeeResponse> Attendees { get; set; }
        public IEnumerable<ConferenceResponse> Conferences { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Attendees = await _apiClient.GetAllAttendeesAsync();

            if (Attendees == null)
            {
                return NotFound();
            }

            Conferences = (await _apiClient.GetAllConferencesAsync())
                                            .Where(c => Attendees.SelectMany(a => a.Sessions)
                                                                .Any(s => s.ConferenceId == c.ID));

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(string username)
        {
            await _apiClient.DeleteAttendeeAsync(username);

            return RedirectToPage();
        }
    }
}