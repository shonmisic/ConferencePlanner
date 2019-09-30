using System.Collections.Generic;

namespace ConferenceDTO
{
    public class ConferenceRequest : Conference
    {
        public ICollection<Session> Sessions { get; set; } = new List<Session>();

        public ICollection<Track> Tracks { get; set; } = new List<Track>();

        public ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();

        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
