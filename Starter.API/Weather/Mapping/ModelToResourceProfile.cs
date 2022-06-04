using AutoMapper;
using Starter.API.Weather.Domain.Models;
using Starter.API.Weather.Resources;

namespace Starter.API.Weather.Mapping;

public class ModelToResourceProfile : Profile {
  public ModelToResourceProfile() {
    CreateMap<Forecast, ForecastResource>();
  }
}
