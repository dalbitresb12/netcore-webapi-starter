using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Starter.API.Shared.Extensions;
using Starter.API.Weather.Domain.Models;
using Starter.API.Weather.Domain.Services;
using Starter.API.Weather.Resources;

namespace Starter.API.Weather.Controllers;

[ApiController]
[Route("[controller]")]
public class ForecastController : ControllerBase {
  private readonly IForecastService service;
  private readonly IMapper mapper;

  public ForecastController(IForecastService service, IMapper mapper) {
    this.service = service;
    this.mapper = mapper;
  }

  [HttpGet]
  public async Task<IEnumerable<ForecastResource>> GetAll() {
    var forecasts = await service.ListAll();
    return mapper.Map<IEnumerable<Forecast>, IEnumerable<ForecastResource>>(forecasts);
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] ForecastRequest resource) {
    if (!ModelState.IsValid) {
      return BadRequest(ModelState.GetErrorMessages());
    }

    var forecast = mapper.Map<ForecastRequest, Forecast>(resource);
    var result = await service.Create(forecast);
    return result.ToResponse<ForecastResource>(this, mapper);
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Put(int id, [FromBody] ForecastRequest resource) {
    if (!ModelState.IsValid) {
      return BadRequest(ModelState.GetErrorMessages());
    }

    var forecast = mapper.Map<ForecastRequest, Forecast>(resource);
    var result = await service.Update(id, forecast);
    return result.ToResponse<ForecastResource>(this, mapper);
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteAsync(int id) {
    var result = await service.Delete(id);
    return result.ToResponse<ForecastResource>(this, mapper);
  }
}
