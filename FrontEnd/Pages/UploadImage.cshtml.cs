using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Infrastructure;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class UploadImageModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public UploadImageModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [Required]
        [Display(Name = "Image to upload")]
        [BindProperty]
        public ImageUpload ImageUpload { get; set; }

        public string AttendeeUsername { get; set; }

        public void OnGet(string attendeeUsername)
        {
            AttendeeUsername = attendeeUsername;
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
                    Name = ImageUpload.Name,
                    Content = imageContent,
                    ImageType = image.ContentType
                };

                if (AttendeeUsername == null)
                {
                    TempData.Set(TempDataKey.NewImage, imageRequest);
                }
                else
                {
                    await _apiClient.AddImageToAttendeeAsync(AttendeeUsername, imageRequest);
                }

                return RedirectToPage(Request.Headers["Referer"].ToString());
            }
        }
    }
}