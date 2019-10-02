using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Conference : ConferenceDTO.Conference
    {
        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
        public virtual ICollection<ConferenceSpeaker> ConferenceSpeakers { get; set; } = new List<ConferenceSpeaker>();
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<ConferenceAttendee> ConferenceAttendees { get; set; } = new List<ConferenceAttendee>();
    }
}
