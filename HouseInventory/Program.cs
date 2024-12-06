
using HouseInventory.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Setting up the configuration for the logger service.
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // Add services to the container.
            builder.Services.AddAuthentication();
            builder.Services.AddCustomIdentityConfiguration();
            builder.Services.AddDatabaseConnection(configuration);
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddCustomServices();
            builder.Services.AddCustomActionFilters();
            builder.Services.AddCustomOptions(configuration);

            builder.Services.AddCorsConfiguration();
            builder.Services.ConfigureIISIntegration();

            builder.Services.ConfigureLoggerService();
            builder.Services.ConfigureJWTAuthentication(configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else 
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
