using Microsoft.OpenApi.Models;
using Prestadito.Security.API.Controller;
using Prestadito.Security.Application.Manager.Endpoints;
using Prestadito.Security.Application.Manager.Extensions;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Services.Interfaces;
using Prestadito.Security.Application.Services.Services;
using Prestadito.Security.Infrastructure.Data.Settings;
using Prestadito.Security.Infrastructure.MainModule.Extensions;

namespace Prestadito.Security.API
{
    public static class WebApplicationHelper
    {
        public static WebApplication CreateWebApplication(this WebApplicationBuilder builder)
        {
            var provider = builder.Services.BuildServiceProvider();

            var configuration = provider.GetRequiredService<IConfiguration>();

            builder.Services.AddMongoDbContext(configuration);

            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddScoped<IUsersController, UsersController>();

            builder.Services.AddValidators();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Prestadito.Micro.Security.API",
                    Description = "ASP.NET Core Web API Control Schedule System",
                    TermsOfService = new Uri("https://prestadito.pe/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Prestadito.pe",
                        Email = "contacto@prestadito.pe",
                        Url = new Uri("https://prestadito.pe"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://prestadito.pe"),
                    }
                });
            });

            builder.Services.AddHealthChecks()
                .AddCheck<MongoDBHealthCheck>(nameof(MongoDBHealthCheck));

            return builder.Build();
        }

        public static WebApplication ConfigureWebApplication(this WebApplication app)
        {
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("v1/swagger.json", "Prestadito.Micro.Security.API");
                });
            //}

            app.UseSecurityEndpoints();

            return app;
        }
    }
}
