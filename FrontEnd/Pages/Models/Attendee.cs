using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Pages.Models
{
    public class Attendee : ConferenceDTO.Attendee
    {
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Email address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}
