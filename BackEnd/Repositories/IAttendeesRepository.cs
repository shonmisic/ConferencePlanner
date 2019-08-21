using System.Threading;
using System.Threading.Tasks;
using BackEnd.Data;

namespace BackEnd.Repositories
{
    public interface IAttendeesRepository
    {
        Task<Attendee> GetByUsernameAsync(string username, CancellationToken cancellationToken = default(CancellationToken));
        Task<Attendee> AddAsync(Attendee attendee, CancellationToken cancellationToken = default(CancellationToken));
        Task<Attendee> AddSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken));
    }
}