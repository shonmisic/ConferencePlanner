using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Models
{
    public class Attendee : ConferenceDTO.Attendee
    {
        [DisplayName("First name")]
        public override string FirstName { get; set; }

        [DisplayName("Last name")]
        public override string LastName { get; set; }

        [DisplayName("Email address")]
        [DataType(DataType.EmailAddress)]
        public override string EmailAddress { get; set; }
    }
}
