using System;
using System.Collections.Generic;

namespace ConferenceDTO
{
    [Serializable]
    public class TrackResponse : Track
    {
        public Conference Conference { get; set; }

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
