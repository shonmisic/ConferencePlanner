using BackEnd.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public interface IConferencesRepository
    {
        Task<Conference> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        IQueryable<Conference> GetAll(CancellationToken cancellationToken = default(CancellationToken));
        Task<Conference> AddAsync(Conference conference, CancellationToken cancellationToken = default(CancellationToken));
        Task<Conference> UpdateAsync(Conference conference, CancellationToken cancellationToken = default(CancellationToken));
        Task<Conference> DeleteByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
