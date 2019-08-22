using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Pages.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class ImagesModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public ImagesModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [Required]
        [Display(Name = "Image to upload")]
        [BindProperty]
        public ImageUpload ImageUpload { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<ImageResponse> Images { get; set; }

        public async Task OnGetAsync()
        {
            Images = await _apiClient.GetImagesAsync();

            var first = Images.FirstOrDefault();

            if (first != null)
            {
                ImageUrl = string.Format("data:/image" + first.ImageType + ";base64,{0}", first.Content);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var ms = new MemoryStream())
            {
                var image = ImageUpload.Image;
                await image.CopyToAsync(ms);
                var imageBytes = ms.ToArray();
                var imageContent = Convert.ToBase64String(imageBytes);

                var imageRequest = new ImageRequest
                {
                    Name = image.Name,
                    Content = imageContent,
                    ImageType = image.ContentType
                };

                await _apiClient.AddImageToAttendeeAsync(User.Identity.Name, imageRequest);

                return RedirectToPage();
            }
        }
    }
}