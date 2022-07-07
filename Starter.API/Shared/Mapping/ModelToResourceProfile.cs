using AutoMapper;
using Starter.API.Weather.Mapping;

namespace Starter.API.Shared.Mapping;

public class ModelToResourceProfile : Profile {
  public ModelToResourceProfile() {
    ForecastModelToResourceProfile.Register(this);
  }
}
