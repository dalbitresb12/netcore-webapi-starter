using Microsoft.EntityFrameworkCore;
using Starter.API.Weather.Domain.Repositories;
using Starter.API.Weather.Domain.Services;
using Starter.API.Weather.Mapping;
using Starter.API.Weather.Persistence.Contexts;
using Starter.API.Weather.Persistence.Repositories;
using Starter.API.Weather.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database connection
var connectionString = builder.Configuration["DbConnectionString"];
var serverVersion = ServerVersion.AutoDetect(connectionString);
var logLevel = builder.Environment.IsProduction() ? LogLevel.Warning : LogLevel.Information;
var enableDebugInfo = !builder.Environment.IsProduction();

builder.Services.AddDbContext<AppDbContext>(
  options => options.UseMySql(connectionString, serverVersion)
    .LogTo(Console.WriteLine, logLevel)
    .EnableSensitiveDataLogging(enableDebugInfo)
    .EnableDetailedErrors(enableDebugInfo)
);

// Add lowercase routes
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Dependency Injection configuration
builder.Services.AddScoped<IForecastRepository, ForecastRepository>();
builder.Services.AddScoped<IForecastService, ForecastService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(ModelToResourceProfile), typeof(ResourceToModelProfile));

var app = builder.Build();

// Database objects validation
if (app.Environment.IsDevelopment()) {
  var scope = app.Services.CreateScope();
  var context = scope.ServiceProvider.GetService<AppDbContext>();
  context?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  _ = app.UseSwagger();
  _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
