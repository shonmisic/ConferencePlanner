using System;
using System.Collections.Generic;

namespace ConferenceDTO
{
    [Serializable]
    public class SessionResponse : Session
    {
        public Track Track { get; set; }

        public ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
