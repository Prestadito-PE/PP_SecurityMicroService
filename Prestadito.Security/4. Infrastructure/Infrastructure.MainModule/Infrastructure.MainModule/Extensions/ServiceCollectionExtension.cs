namespace Prestadito.Security.Infrastructure.MainModule.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            //MongoDB
            services.Configure<SecurityDBSettings>(configuration.GetSection(nameof(SecurityDBSettings)));

            return services;
        }
    }
}
