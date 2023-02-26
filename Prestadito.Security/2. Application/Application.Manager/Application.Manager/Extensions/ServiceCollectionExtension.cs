using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Prestadito.Security.Application.Dto.User;
using Prestadito.Security.Application.Manager.Validators;

namespace Prestadito.Security.Application.Manager.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserDTO>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUserDTO>, UpdateUserValidator>();

            return services;
        }
    }
}
