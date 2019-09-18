using System;

namespace ConferenceDTO
{
    [Serializable]
    public class ImageResponse : Image
    {
        public Attendee Attendee { get; set; }
    }
}
