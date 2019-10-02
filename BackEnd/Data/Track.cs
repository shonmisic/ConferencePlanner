using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Track : ConferenceDTO.Track
    {
        public Conference Conference { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
