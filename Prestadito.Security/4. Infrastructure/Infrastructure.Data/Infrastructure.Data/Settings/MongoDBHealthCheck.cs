namespace Prestadito.Security.Infrastructure.Data.Settings
{
    public class MongoDBHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            bool result = false;

            if (result)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}
