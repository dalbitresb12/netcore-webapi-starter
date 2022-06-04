using AutoMapper;
using Starter.API.Weather.Domain.Models;
using Starter.API.Weather.Resources;

namespace Starter.API.Weather.Mapping;

public class ResourceToModelProfile : Profile {
  public ResourceToModelProfile() {
    CreateMap<ForecastRequest, Forecast>();
  }
}
