using BackEnd.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public interface ITracksRepository
    {
        Task<ICollection<Track>> GetByConferenceIdAsync(int conferenceId);
    }
}
