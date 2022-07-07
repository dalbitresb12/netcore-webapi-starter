using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Starter.API.Shared.Extensions;
using Starter.API.Shared.Injection;
using Starter.API.Shared.Mapping;
using Starter.API.Shared.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container with path prefix
builder.Services.AddControllers(options => options.UseGeneralRoutePrefix("api/v1"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
  options => options.SwaggerDoc(
    "v1",
    new OpenApiInfo {
      Title = "Weather Forecast API",
      Version = "v1",
      Description = "An ASP.NET Core Web API for managing Weather Forecast items",
      TermsOfService = new Uri("https://example.com/terms"),
      Contact = new OpenApiContact {Name = "Example Contact", Url = new Uri("https://example.com/contact"),},
      License = new OpenApiLicense {Name = "MIT", Url = new Uri("https://choosealicense.com/licenses/mit/"),},
    }
  )
);

// Add database connection
var connectionString = builder.Configuration["DbConnectionString"];
var logLevel = builder.Environment.IsProduction() ? LogLevel.Warning : LogLevel.Information;
var enableDebugInfo = !builder.Environment.IsProduction();

builder.Services.AddDbContext<AppDbContext>(
  options => {
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    options.UseMySql(connectionString, serverVersion)
      .LogTo(Console.WriteLine, logLevel)
      .EnableSensitiveDataLogging(enableDebugInfo)
      .EnableDetailedErrors(enableDebugInfo);
  }
);

// Add lowercase routes
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Dependency Injection configuration
AppInjections.Register(builder.Services);

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(ModelToResourceProfile), typeof(ResourceToModelProfile));

var app = builder.Build();

var useMigrations = builder.Configuration.GetValue<bool>("UseMigrations");

// Database objects validation
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetService<AppDbContext>();

if (useMigrations)
  context?.Database.Migrate();
else
  context?.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  _ = app.UseSwagger();
  _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// ReSharper disable once ClassNeverInstantiated.Global
[SuppressMessage("Design", "CA1050:Declare types in namespaces")]
public partial class Program {}
