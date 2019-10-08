using BackEnd.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public interface IAttendeesRepository
    {
        Task<Attendee> GetByUsernameAsync(string username, CancellationToken cancellationToken = default(CancellationToken));
        Task<Attendee> AddAsync(Attendee attendee, CancellationToken cancellationToken = default(CancellationToken));
        Task<Attendee> AddSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken));
        Task<Attendee> AddConferenceAsync(string username, int conferenceId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveConferenceAsync(string username, int conferenceId, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateAsync(Attendee attendee,CancellationToken cancellationToken = default(CancellationToken));
        IQueryable<Attendee> GetAll();
    }
}