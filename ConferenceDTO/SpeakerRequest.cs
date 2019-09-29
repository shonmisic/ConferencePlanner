using System.Collections.Generic;

namespace ConferenceDTO
{
    public class SpeakerRequest : Speaker
    {
        public ImageRequest Image { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
