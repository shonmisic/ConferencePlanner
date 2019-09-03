using System;
using System.Collections.Generic;

namespace ConferenceDTO
{
    [Serializable]
    public class SpeakerResponse : Speaker
    {
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
