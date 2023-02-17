namespace Prestadito.Security.Application.Manager.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserDTO>, CreateUserValidator>();

            return services;
        }
    }
}
