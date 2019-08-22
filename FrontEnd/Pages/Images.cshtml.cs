using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Pages.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

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

        public IEnumerable<Models.Image> Images { get; set; }

        public async Task OnGetAsync()
        {
            var images = await _apiClient.GetImagesAsync();

            Images = images.Where(i => i.ImageType == "image/png" || i.ImageType == "image/jpeg")
                           .Select(ConvertToImages());
        }

        private static Func<ImageResponse, Models.Image> ConvertToImages()
        {
            return image =>
            {
                using (var ms = new MemoryStream(Convert.FromBase64String(image.Content)))
                {
                    IImageDecoder decoder = new PngDecoder();
                    if (image.ImageType == "image/jpeg")
                    {
                        decoder = new JpegDecoder();
                    }
                    using (Image<Rgba32> imageSharp = Image.Load(ms, decoder))
                    {
                        imageSharp.Mutate(ctx => ctx.Resize(imageSharp.Width / 2, imageSharp.Height / 2));

                        using (var outputStream = new MemoryStream())
                        {
                            IImageEncoder encoder = new PngEncoder();
                            if (image.ImageType == "image/jpeg")
                            {
                                encoder = new JpegEncoder();
                            }
                            imageSharp.Save(outputStream, encoder);
                            outputStream.Position = 0;
                            var content = Convert.ToBase64String(outputStream.ToArray());

                            return new Models.Image
                            {
                                ImageUrl = string.Format("data:/" + image.ImageType + ";base64,{0}", content),
                                Name = image.Name
                            };
                        }
                    }
                }
            };
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

                await _apiClient.AddImageToAttendeeAsync(User.Identity.Name, imageRequest);

                return RedirectToPage();
            }
        }
    }
}