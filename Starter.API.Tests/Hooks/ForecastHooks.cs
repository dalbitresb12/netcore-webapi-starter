using System.Globalization;
using BoDi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Starter.API.Weather.Domain.Repositories;
using Starter.API.Weather.Persistence.Contexts;
using TechTalk.SpecFlow.Assist.ValueRetrievers;

namespace Starter.API.Tests.Hooks;

[Binding]
public class ForecastHooks {
  private readonly IObjectContainer objectContainer;

  public ForecastHooks(IObjectContainer objectContainer) {
    this.objectContainer = objectContainer;
  }

  [BeforeScenario]
  public async Task RegisterServices() {
    var factory = GetWebApplicationFactory();
    await ClearData(factory);
    objectContainer.RegisterInstanceAs(factory);
    var repository = factory.Services.GetService(typeof(IForecastRepository)) as IForecastRepository;
    objectContainer.RegisterInstanceAs(repository);
    var unitOfWork = factory.Services.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
    objectContainer.RegisterInstanceAs(unitOfWork);
  }

  [BeforeTestRun]
  public static void BeforeTestRun() {
    DateTimeValueRetriever.DateTimeStyles = DateTimeStyles.AssumeUniversal;
  }

  private static WebApplicationFactory<Program> GetWebApplicationFactory() {
    return new WebApplicationFactory<Program>().WithWebHostBuilder(
      builder => {
        builder.ConfigureTestServices(
          services => {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor is not null) {
              services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(
              options => options.UseInMemoryDatabase("InMemoryTestDatabase")
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
            );

            var provider = services.BuildServiceProvider();
            var scope = provider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var ctx = scopedServices.GetRequiredService<AppDbContext>();

            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
          }
        );

        builder.UseEnvironment("Testing");
      }
    );
  }

  private static async Task ClearData(WebApplicationFactory<Program> factory) {
    if (factory.Services.GetService(typeof(IForecastRepository)) is not IForecastRepository repository) {
      return;
    }

    var entities = await repository.ListAll();
    foreach (var entity in entities) {
      repository.Remove(entity);
    }
  }
}
