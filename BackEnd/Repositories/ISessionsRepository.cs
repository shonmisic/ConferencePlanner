using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackEnd.Data;

namespace BackEnd.Repositories
{
    public interface ISessionsRepository
    {
        Task<IQueryable<Session>> GetByConferenceIdAsync(int conferenceId, CancellationToken cancellationToken = default(CancellationToken));
        Task<Session> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        Task<Session> AddAsync(Session session, CancellationToken cancellationToken = default(CancellationToken));
        Task<Session> UpdateAsync(ConferenceDTO.Session session, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        IQueryable<Session> GetAll();
        Task<IQueryable<Session>> GetByTrackIdAsync(int trackId, CancellationToken cancellationToken = default(CancellationToken));
    }
}