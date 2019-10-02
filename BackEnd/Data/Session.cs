using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Session : ConferenceDTO.Session
    {
        public Conference Conference { get; set; }
        public virtual ICollection<SessionSpeaker> SessionSpeakers { get; set; } = new List<SessionSpeaker>();
        public Track Track { get; set; }
        public virtual ICollection<SessionTag> SessionTags { get; set; } = new List<SessionTag>();
        public virtual ICollection<SessionAttendee> SessionAttendees { get; set; } = new List<SessionAttendee>();
    }
}
