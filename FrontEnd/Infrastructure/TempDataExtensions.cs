using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace FrontEnd.Infrastructure
{
    public static class TempDataExtensions
    {
        public static void Set<T>(this ITempDataDictionary tempData, TempDataKey key, T value) where T : class
        {
            tempData[key.ToString()] = JsonConvert.SerializeObject(value);
        }
        public static T Get<T>(this ITempDataDictionary tempData, TempDataKey key) where T : class
        {
            tempData.TryGetValue(key.ToString(), out object o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string) o);
        }
    }
}
