using FrontEnd.Infrastructure;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Pages.Admin
{
    public class EditAttendeeModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public EditAttendeeModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Attendee Attendee { get; set; }
        public ICollection<ConferenceDTO.ConferenceResponse> Conferences { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGet(string username)
        {
            var attendee = await _apiClient.GetAttendeeAsync(username);
            Attendee = new Attendee
            {
                ID = attendee.ID,
                EmailAddress = attendee.EmailAddress,
                FirstName = attendee.FirstName,
                LastName = attendee.LastName,
                UserName = attendee.UserName,
                Url = attendee.Url,
                Conferences = attendee.Conferences.Select(c => c.MapConferenceResponse()).ToList()
            };

            Conferences = await _apiClient.GetAllConferencesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Attendee updated successfully";

            await _apiClient.PutAttendeeAsync(Attendee);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var attendee = await _apiClient.GetAttendeeAsync(Attendee.UserName);

            if (attendee != null)
            {
                await _apiClient.DeleteAttendeeAsync(attendee.UserName);
            }

            Message = "Attendee deleted successfully";

            return RedirectToPage("/Admin/Attendees");
        }

        public async Task<IActionResult> OnPostRemoveConferenceAsync(int conferenceId)
        {
            await _apiClient.RemoveConferenceFromAttendeeAsync(Attendee.UserName, conferenceId);

            var conference = Conferences.SingleOrDefault(c => c.ID == conferenceId);

            Conferences.Remove(conference);

            Message = "Conference removed successfully";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddConferenceAsync(int conferenceId)
        {
            await _apiClient.AddConferenceToAttendeeAsync(Attendee.UserName, conferenceId);

            return RedirectToPage();
        }

        public bool IsAttendingConference(int conferenceId)
        {
            return Attendee.Conferences.Any(c => c.ID == conferenceId);
        }
    }
}