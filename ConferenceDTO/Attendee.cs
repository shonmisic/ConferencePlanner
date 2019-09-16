using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDTO
{
    [Serializable]
    public class Attendee
    {
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public virtual string EmailAddress { get; set; }

        public string Url { get; set; }
    }
}
