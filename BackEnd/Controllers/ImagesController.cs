using BackEnd.Infrastructure;
using BackEnd.Repositories;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly IDistributedCache _cache;

        private static readonly string _getImages = "GetImages";

        public ImagesController(IImagesRepository imagesRepository, IDistributedCache cache)
        {
            _imagesRepository = imagesRepository;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ImageResponse>>> Get()
        {
            var cachedValue = await _cache.GetAsync(_getImages);

            var result = cachedValue.FromByteArray<List<ImageResponse>>();

            if (result == null)
            {
                var images = await _imagesRepository.GetImagesAsync();

                result = images.Select(i => i.MapImageResponse()).ToList();

                await CacheValue(_getImages, result);
            }

            return result;
        }

        private async Task CacheValue<T>(string key, T result)
        {
            var valueToCache = result.ToByteArray();
            var options = new DistributedCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(1));
            await _cache.SetAsync(key, valueToCache, options);
        }
    }
}