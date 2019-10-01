using Microsoft.Extensions.DependencyInjection;
using System;

namespace FrontEnd.Services
{
    public class ApiClientFactory : IApiClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IApiClient Create()
        {
            return _serviceProvider.GetRequiredService<IApiClient>();
        }
    }
}
