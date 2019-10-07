using ConferenceDTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontEnd.Pages.Shared
{
    public class SessionsPartialModel
    {
        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> SessionsGroupedByTime { get; set; }
        public int SelectedConferenceID { get; set; }
        public string AttendeeUsername { get; set; }

        public bool IsAttendingSession(int sessionId)
        {
            var session = SessionsGroupedByTime.SelectMany(s => s)
                        .SingleOrDefault(s => s.ID == sessionId);
            return session
                        .Attendees
                        .Any(a => a.UserName == AttendeeUsername);
        }
    }
}
