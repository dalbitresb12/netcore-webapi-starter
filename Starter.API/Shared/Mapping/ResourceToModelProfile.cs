using AutoMapper;
using Starter.API.Weather.Mapping;

namespace Starter.API.Shared.Mapping;

public class ResourceToModelProfile : Profile {
  public ResourceToModelProfile() {
    ForecastResourceToModelProfile.Register(this);
  }
}
