using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Infrastructure
{
    public static class HttpContentHelper
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var str = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
