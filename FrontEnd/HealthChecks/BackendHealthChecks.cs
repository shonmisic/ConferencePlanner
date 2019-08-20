using System.Threading;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FrontEnd.HealthChecks
{
    public class BackendHealthChecks : IHealthCheck
    {
        private readonly IApiClient _apiClient;

        public BackendHealthChecks(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await _apiClient.CheckHealthAsync())
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
    }
}
