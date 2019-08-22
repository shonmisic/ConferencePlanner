using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConferenceDTO
{
    public class Image
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTimeOffset UploadDate { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string ImageType { get; set; }

        [Required]
        public int AttendeeId { get; set; }
    }
}
