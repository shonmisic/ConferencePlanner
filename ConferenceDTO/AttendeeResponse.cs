using System;
using System.Collections.Generic;

namespace ConferenceDTO
{
    [Serializable]
    public class AttendeeResponse : Attendee
    {
        public ICollection<Conference> Conferences { get; set; } = new List<Conference>();

        public ICollection<Session> Sessions { get; set; } = new List<Session>();

        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
