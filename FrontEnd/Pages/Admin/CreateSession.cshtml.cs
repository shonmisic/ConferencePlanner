using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Infrastructure;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public Session Session { get; set; }

        public ICollection<SelectListItem> ConferenceItemList { get; set; }
        public ICollection<SelectListItem> TrackItemList { get; set; }

        public async Task OnGetAsync()
        {
            var conferences = await _apiClient.GetAllConferencesAsync();

            ConferenceItemList = 
                conferences.Select(c => new SelectListItem
                {
                    Value = c.ID.ToString(),
                    Text = c.Name
                })
                .ToList();

            TrackItemList = 
                conferences.SelectMany(c => c.Tracks)
                            .Select(t => new SelectListItem
                            {
                                Value = t.ID.ToString(),
                                Text = t.Name
                            })
                            .ToList();
        }

        public IActionResult OnPostAsync()
        {
            TempData.Set(TempDataKey.NewSession, Session);

            return RedirectToPage("/Admin/CreateSpeaker");
        }
    }
}