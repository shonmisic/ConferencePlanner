using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Attendee : ConferenceDTO.Attendee
    {
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<SessionAttendee> SessionAttendees { get; set; }
        public virtual ICollection<ConferenceAttendee> ConferenceAttendees { get; set; }
    }
}
