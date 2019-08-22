using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackEnd.Data;

namespace BackEnd.Repositories
{
    public interface IImagesRepository
    {
        Task<ICollection<Image>> GetImagesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}