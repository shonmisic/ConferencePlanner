using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FrontEnd.Models
{
    public class ImageUpload
    {
        [DisplayName("File name")]
        public string Name { get; set; }

        [DisplayName("Image to upload")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
