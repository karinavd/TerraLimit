using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TerraLimit.Model.Interfaces;
using TerraLimit.Model.Services;
using TerraLimit.Persistence.Data;
using TerraLimit.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddDbContext<EcoState>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<IWaterReadRepository, WaterReadRepository>();
builder.Services.AddScoped<IWeatherReadRepository, WeatherReadRepository>();
builder.Services.AddScoped<IBaseReadRepository, BaseReadRepository>();
builder.Services.AddScoped<IBaseService, BaseService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .WithHeaders("Content-Type", "Authorization");
    });
});

var app = builder.Build();

app.UseCors("FrontendPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
