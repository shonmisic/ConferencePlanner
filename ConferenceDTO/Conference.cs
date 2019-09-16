using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDTO
{
    [Serializable]
    public class Conference
    {
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Url { get; set; }
    }
}
