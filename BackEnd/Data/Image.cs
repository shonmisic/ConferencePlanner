using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class Image : ConferenceDTO.Image
    {
        public Attendee Attendee { get; set; }
    }
}
