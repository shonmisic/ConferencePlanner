using ConferenceDTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontEnd.Pages.Shared
{
    public class SessionsPartialModel
    {
        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; }
        public IEnumerable<(int Offset, DayOfWeek? DayOfWeek)> DayOffsets { get; set; }
        public int CurrentDayOffset { get; set; }
        public int SelectedConferenceID { get; set; }
        public string ParentPagePath { get; set; }
        public string AttendeeUsername { get; set; }
        public int TrackId
        {
            get
            {
                var sessions = Sessions.SelectMany(s => s.Select(sr => sr));
                if (sessions.Select(s => s.TrackId).Distinct().Count() == 1)
                {
                    return sessions.FirstOrDefault(s => s.TrackId.HasValue).TrackId.Value;
                }

                return 0;
            }
        }

        public bool IsAttendingSession(int sessionId)
        {
            var session = Sessions.SelectMany(s => s)
                        .SingleOrDefault(s => s.ID == sessionId);
            return session
                        .Attendees
                        .Any(a => a.UserName == AttendeeUsername);
        }
    }
}
