using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Attendee : ConferenceDTO.Attendee
    {
        public virtual ICollection<AttendeeImage> AttendeeImages { get; set; } = new List<AttendeeImage>();
        public virtual ICollection<SessionAttendee> SessionAttendees { get; set; } = new List<SessionAttendee>();
        public virtual ICollection<ConferenceAttendee> ConferenceAttendees { get; set; } = new List<ConferenceAttendee>();
    }
}
