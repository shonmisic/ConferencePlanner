using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDTO
{
    [Serializable]
    public class Track
    {
        public int ID { get; set; }

        public int ConferenceId { get; set; }

        public string Name { get; set; }
    }
}
