using Starter.API.Shared.Domain.Service.Communication;
using Starter.API.Weather.Domain.Models;

namespace Starter.API.Weather.Domain.Services.Communication;

public class ForecastResponse : BaseResponse<Forecast> {
  public ForecastResponse(string message) : base(message) {}
  public ForecastResponse(Forecast resource) : base(resource) {}
}
