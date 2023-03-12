using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Application.Manager.Validators;

namespace Prestadito.Security.Application.Manager.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<GetUserByIdRequest>, GetUserByIdValidator>();
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidator>();

            services.AddScoped<IValidator<LoginDTO>, LoginValidator>();
            return services;
        }
    }
}
