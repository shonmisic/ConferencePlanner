using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Image : ConferenceDTO.Image
    {
        public virtual ICollection<SpeakerImage> SpeakerImages { get; set; }
        public virtual ICollection<AttendeeImage> AttendeeImages { get; set; }
    }
}
