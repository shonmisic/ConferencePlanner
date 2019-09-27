using System.Collections.Generic;

namespace ConferenceDTO
{
    public class SpeakerRequest : Speaker
    {
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
