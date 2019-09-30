using BackEnd.Data;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public interface IConferencesRepository
    {
        Task<Conference> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
