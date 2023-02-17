namespace Prestadito.Security.Infrastructure.MainModule.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SecurityDBSettings>(configuration.GetSection(nameof(SecurityDBSettings)));
            services.AddSingleton<ISecurityDBSettings>(sp => sp.GetRequiredService<IOptions<SecurityDBSettings>>().Value);
            services.AddSingleton<MongoContext>();

            return services;
        }
    }
}
