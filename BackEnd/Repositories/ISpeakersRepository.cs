using BackEnd.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public interface ISpeakersRepository
    {
        IQueryable<Speaker> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<Speaker> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        Task<Speaker> AddAsync(Speaker speaker, CancellationToken cancellationToken = default(CancellationToken));
        Task<Speaker> UpdateAsync(Speaker speaker, CancellationToken cancellationToken = default(CancellationToken));
        Task<Speaker> RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
