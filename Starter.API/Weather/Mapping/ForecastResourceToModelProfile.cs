using AutoMapper;
using Starter.API.Weather.Domain.Models;
using Starter.API.Weather.Resources;

namespace Starter.API.Weather.Mapping;

public static class ForecastResourceToModelProfile {
  public static void Register(IProfileExpression profile) {
    profile.CreateMap<ForecastRequest, Forecast>();
  }
}
