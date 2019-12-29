using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Attendee : ConferenceDTO.Attendee
    {
        public virtual ICollection<AttendeeImage> AttendeeImages { get; set; }
        public virtual ICollection<SessionAttendee> SessionAttendees { get; set; }
        public virtual ICollection<ConferenceAttendee> ConferenceAttendaees { get; set; }
    }
}
