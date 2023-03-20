using Microsoft.Extensions.DependencyInjection;
using Prestadito.Security.Infrastructure.Proxies.Settings.Interfaces;
using Prestadito.Security.Infrastructure.Proxies.Settings.Proxies;

namespace Prestadito.Security.Infrastructure.Proxies.Settings.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddProxies(this IServiceCollection services)
        {
            services.AddHttpClient<ISettingProxy, SettingProxy>();

            return services;
        }
    }
}
