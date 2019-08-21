﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackEnd.Data;

namespace BackEnd.Repositories
{
    public interface ISessionsRepository
    {
        Task<ICollection<Session>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<Session> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        Task<Session> AddAsync(Session session, CancellationToken cancellationToken = default(CancellationToken));
        Task<Session> UpdateAsync(ConferenceDTO.Session session, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}