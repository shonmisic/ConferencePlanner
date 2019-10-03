using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
    public class SessionsContainingPageModel<T> : PageModel where T : PageModel
    {
        protected readonly IApiClient _apiClient;
        protected readonly ILogger<T> _logger;

        public SessionsContainingPageModel(IApiClient apiClient, ILogger<T> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        protected Task<ICollection<SessionResponse>> GetSessionsAsync(int conferenceId)
        {
            _logger.LogDebug("GetSessionsAsync was called");

            return _apiClient.GetSessionsAsync(conferenceId);
        }

        public async Task<IActionResult> OnPostAddSessionToAttendeeAsync(int sessionId)
        {
            await _apiClient.AddSessionToAttendeeAsync(User.Identity.Name, sessionId);

            var session = await _apiClient.GetSessionAsync(sessionId);

            return RedirectToPage(new { session.ConferenceId, day = session.StartTime?.DayOfWeek });
        }

        public async Task<IActionResult> OnPostRemoveSessionFromAttendeeAsync(int sessionId)
        {
            await _apiClient.RemoveSessionFromAttendeeAsync(User.Identity.Name, sessionId);

            var session = await _apiClient.GetSessionAsync(sessionId);

            return RedirectToPage(new { session.ConferenceId, day = session.StartTime?.DayOfWeek });
        }
    }
}
