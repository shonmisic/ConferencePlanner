using BackEnd.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public interface ITracksRepository
    {
        Task<ICollection<Track>> GetAllByConferenceIdAsync(int conferenceId, CancellationToken cancellationToken = default(CancellationToken));
        Task<Track> AddAsync(Track track, CancellationToken cancellationToken = default(CancellationToken));
        Task<Track> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
