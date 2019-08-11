using System;
using System.Threading.Tasks;
using FrontEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FrontEnd.Services
{
    public class AdminService : IAdminService
    {
        private readonly Lazy<long> _creationKey = new Lazy<long>(() => BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 7));
        private readonly IServiceProvider _serviceProvider;
        private bool _adminExists;

        public AdminService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public long CreationKey => _creationKey.Value;

        public async Task<bool> AllowAdminUserCreationAsync()
        {
            if (_adminExists)
            {
                return false;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                if (await dbContext.Users.AnyAsync(u => u.IsAdmin))
                {
                    _adminExists = true;
                    return false;
                }
            }

            return true;
        }
    }
}
