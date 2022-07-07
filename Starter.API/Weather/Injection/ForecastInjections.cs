using Starter.API.Weather.Domain.Repositories;
using Starter.API.Weather.Domain.Services;
using Starter.API.Weather.Persistence.Repositories;
using Starter.API.Weather.Services;

namespace Starter.API.Weather.Injection;

public static class ForecastInjections {
  public static void Register(IServiceCollection services) {
    services.AddScoped<IForecastRepository, ForecastRepository>();
    services.AddScoped<IForecastService, ForecastService>();
  }
}
