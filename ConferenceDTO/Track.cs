using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConferenceDTO
{
    public class Track
    {
        public int ID { get; set; }

        [Required]
        public int ConferenceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
    }
}
