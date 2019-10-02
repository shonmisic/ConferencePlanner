using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Speaker : ConferenceDTO.Speaker
    {
        public virtual ICollection<ConferenceSpeaker> ConferenceSpeakers { get; set; } = new List<ConferenceSpeaker>();
        public virtual ICollection<SpeakerImage> SpeakerImages { get; set; } = new List<SpeakerImage>();
        public virtual ICollection<SessionSpeaker> SessionSpeakers { get; set; } = new List<SessionSpeaker>();
    }
}
