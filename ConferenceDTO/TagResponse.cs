using System;
using System.Collections.Generic;

namespace ConferenceDTO
{
    [Serializable]
    public class TagResponse : Tag
    {
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
