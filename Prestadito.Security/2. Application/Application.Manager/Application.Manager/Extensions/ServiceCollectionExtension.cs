using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Application.Manager.Controller;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.Validators;
using Prestadito.Security.Infrastructure.Data.Interface;
using Prestadito.Security.Infrastructure.Data.Settings;
using Prestadito.Security.Infrastructure.Data.Utilities;

namespace Prestadito.Security.Application.Manager.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<GetUserByIdRequest>, GetUserByIdValidator>();
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidator>();
            services.AddScoped<IValidator<DisableUserRequest>, DisableUserValidator>();
            services.AddScoped<IValidator<DeleteUserRequest>, DeleteUserValidator>();

            services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
           
            return services;
        }

        public static IServiceCollection AddSecurityControllers(this IServiceCollection services)
        {
            services.AddScoped<IUsersController, UsersController>();
            services.AddScoped<ISessionsController, SessionsController>();
            services.AddTransient<HashService>();
            return services;
        }

        public static IServiceCollection AddJWTSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));
            services.AddSingleton<IJWTSettings>(x => x.GetRequiredService<IOptions<JWTSettings>>().Value);
            services.AddScoped<IJWTHelper, JWTHelper>();

            return services;
        }
    }
}
