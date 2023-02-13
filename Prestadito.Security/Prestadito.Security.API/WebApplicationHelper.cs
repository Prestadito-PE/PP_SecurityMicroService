using Prestadito.Security.API.Controller;

namespace Prestadito.Security.API
{
    public static class WebApplicationHelper
    {
        readonly static string myCors = "myCors";

        public static WebApplication CreateWebApplication(this WebApplicationBuilder builder)
        {

            var provider = builder.Services.BuildServiceProvider();

            var configuration = provider.GetRequiredService<IConfiguration>();

            builder.Services.AddDbContexts(configuration);
            builder.Services.AddSingleton<ISecurityDBSettings>(sp => sp.GetRequiredService<IOptions<SecurityDBSettings>>().Value);

            builder.Services.AddSingleton<MongoContext>();

            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddScoped<IUsersController, UsersController>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Prestadio.Micro.Security.API",
                });
            });

            builder.Services.AddHealthChecks()
                .AddCheck<MongoDBHealthCheck>(nameof(MongoDBHealthCheck));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: myCors,
                    policy =>
                    {
                        policy.WithOrigins("https://localhost").AllowAnyMethod();
                    });
            });

            return builder.Build();
        }

        public static WebApplication ConfigureWebApplication(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(myCors);

            app.UseEndpoints();

            return app;
        }
    }
}
