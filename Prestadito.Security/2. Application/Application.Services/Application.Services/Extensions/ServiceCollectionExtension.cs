using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prestadito.Security.Application.Services.Utilities;
using Prestadito.Security.Infrastructure.Data.Interface;
using Prestadito.Security.Infrastructure.Data.Settings;

namespace Prestadito.Security.Application.Services.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJWTSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));
            services.AddSingleton<IJWTSettings>(x => x.GetRequiredService<IOptions<JWTSettings>>().Value);
            services.AddSingleton<JWTHelper>();

            return services;
        }
    }
}
