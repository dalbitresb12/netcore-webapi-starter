using Starter.API.Shared.Extensions;
using Starter.API.Weather.Domain.Models;
using Starter.API.Weather.Domain.Repositories;
using Starter.API.Weather.Domain.Services;
using Starter.API.Weather.Domain.Services.Communication;

namespace Starter.API.Weather.Services;

public class ForecastService : IForecastService {
  private readonly IForecastRepository repository;
  private readonly IUnitOfWork unitOfWork;

  public ForecastService(IForecastRepository repository, IUnitOfWork unitOfWork) {
    this.repository = repository;
    this.unitOfWork = unitOfWork;
  }

  public Task<IEnumerable<Forecast>> ListAll() {
    return repository.ListAll();
  }

  public async Task<ForecastResponse> Create(Forecast forecast) {
    try {
      await repository.Add(forecast);
      await unitOfWork.Complete();
      return new ForecastResponse(forecast);
    } catch (Exception e) {
      return new ForecastResponse($"An error occurred while saving the forecast: {e.Message}");
    }
  }

  public async Task<ForecastResponse> Update(long id, Forecast forecast) {
    var current = await repository.FindById(id);
    if (current == null) {
      return new ForecastResponse("Forecast not found");
    }

    forecast.CopyProperties(current);

    try {
      repository.Update(current);
      await unitOfWork.Complete();
      return new ForecastResponse(current);
    } catch (Exception e) {
      return new ForecastResponse($"An error occurred while updating the forecast: {e.Message}");
    }
  }

  public async Task<ForecastResponse> Delete(long id) {
    var current = await repository.FindById(id);
    if (current == null) {
      return new ForecastResponse("Forecast not found");
    }

    try {
      repository.Remove(current);
      await unitOfWork.Complete();
      return new ForecastResponse(current);
    } catch (Exception e) {
      return new ForecastResponse($"An error occurred while deleting the forecast: {e.Message}");
    }
  }
}
