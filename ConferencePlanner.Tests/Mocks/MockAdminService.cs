using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;

namespace ConferencePlanner.Tests.Mocks
{
    public class MockAdminService : IAdminService
    {
        public long CreationKey => throw new NotImplementedException();

        public Task<bool> AllowAdminUserCreationAsync()
        {
            throw new NotImplementedException();
        }
    }
}
