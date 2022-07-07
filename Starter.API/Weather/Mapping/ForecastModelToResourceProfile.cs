using AutoMapper;
using Starter.API.Weather.Domain.Models;
using Starter.API.Weather.Resources;

namespace Starter.API.Weather.Mapping;

public static class ForecastModelToResourceProfile {
  public static void Register(IProfileExpression profile) {
    profile.CreateMap<Forecast, ForecastResource>();
  }
}
