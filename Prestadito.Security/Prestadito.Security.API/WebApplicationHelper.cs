namespace Prestadito.Security.API
{
    public static class WebApplicationHelper
    {
        readonly static string myCors = "myCors";

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
                    Title = "Prestadio.Micro.Security.API",
                });
            });

            builder.Services.AddHealthChecks()
                .AddCheck<MongoDBHealthCheck>(nameof(MongoDBHealthCheck));

            builder.Services.AddCors(options =>
            {
                var urlList = configuration.GetSection("AllowedOrigin").GetChildren().Select(c => c.Value).ToArray();
                options.AddPolicy(myCors,
                    builder => builder.WithOrigins(urlList)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());

                //options.AddPolicy(name: myCors,
                //    policy =>
                //    {
                //        policy.WithOrigins("https://localhost").AllowAnyMethod();
                //    });
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
