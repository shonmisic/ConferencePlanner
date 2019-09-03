using Microsoft.Extensions.Caching.Memory;

namespace FrontEnd.Infrastructure
{
    public class MemoryCacheSingleton
    {
        public MemoryCache Cache { get; set; }

        public MemoryCacheSingleton()
        {
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 10240
            });
        }
    }
}
