using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ImagesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Image>> GetImagesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Images.Include(i => i.AttendeeImages)
                                            .ThenInclude(ai => ai.Attendee)
                                        .Include(i => i.SpeakerImages)
                                            .ThenInclude(si => si.Speaker)
                                        .ToListAsync();
        }
    }
}
