using Starter.API.Shared.Persistence.Repositories;
using Starter.API.Weather.Domain.Repositories;
using Starter.API.Weather.Injection;

namespace Starter.API.Shared.Injection;

public static class AppInjections {
  public static void Register(IServiceCollection services) {
    ForecastInjections.Register(services);

    services.AddScoped<IUnitOfWork, UnitOfWork>();
  }
}
