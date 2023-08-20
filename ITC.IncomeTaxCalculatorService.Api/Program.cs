using ITC.IncomeTaxCalculatorService.ApplicationCore;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces;
using ITC.IncomeTaxCalculatorService.ApplicationCore.Services;
using ITC.IncomeTaxCalculatorService.Infrastructure.Data;
using ITC.IncomeTaxCalculatorService.Infrastructure.Data.Repositories;
using ITC.IncomeTaxCalculatorService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ITC.IncomeTaxCalculatorService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: Constants.AllowedSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins(builder.Configuration.GetValue<string>(Constants.AllowedSpecificOrigins))
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                  });
            });

            builder.Logging.AddApplicationInsights(
                configureTelemetryConfiguration: (config) =>
                config.ConnectionString = builder.Configuration.GetConnectionString("AppInsightsConnectionString"),
                configureApplicationInsightsLoggerOptions: (options) => { });

            builder.Services.AddDbContext<TaxCalculatorContext>(ob => ob.UseSqlServer(builder.Configuration.GetConnectionString("TaxCalculatorConnection")));

            // Add services to the container.
            builder.Services.AddScoped<ITaxCalculationService, TaxCalculationService>();
            builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            builder.Services.AddScoped<ITaxBandRepository, TaxBandRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(Constants.AllowedSpecificOrigins);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}