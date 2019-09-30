using System.IO;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public abstract class DataLoader
    {
        public abstract Task<Conference> LoadDataAsync(string conferenceName, Stream fileStream);
    }

}