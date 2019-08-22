using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Infrastructure;
using BackEnd.Repositories;
using ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesRepository _imagesRepository;

        public ImagesController(IImagesRepository imagesRepository)
        {
            _imagesRepository = imagesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ImageResponse>>> Get()
        {
            var images = await _imagesRepository.GetImagesAsync();

            return images.Select(i => i.MapImageResponse()).ToList();
        }
    }
}