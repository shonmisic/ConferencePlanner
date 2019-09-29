using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDTO
{

    [Serializable]
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

        public string Url { get; set; }
    }
}
