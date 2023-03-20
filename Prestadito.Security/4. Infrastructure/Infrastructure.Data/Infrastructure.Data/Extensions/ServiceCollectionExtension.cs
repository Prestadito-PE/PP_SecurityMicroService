using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prestadito.Security.Infrastructure.Data.Context;
using Prestadito.Security.Infrastructure.Data.Interface;
using Prestadito.Security.Infrastructure.Data.Repositories;
using Prestadito.Security.Infrastructure.Data.Settings;

namespace Prestadito.Security.Infrastructure.Data.Extensions
{
    public static class ServiceCollectionExtension
    {
        private static IServiceCollection AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SecurityDBSettings>(configuration.GetSection(nameof(SecurityDBSettings)));
            services.AddSingleton<ISecurityDBSettings>(sp => sp.GetRequiredService<IOptions<SecurityDBSettings>>().Value);
            services.AddScoped<IMongoContext, MongoContext>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            return services;
        }

        public static IServiceCollection AddDBServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDbContext(configuration);
            services.AddRepositories();

            return services;
        }
    }
}
