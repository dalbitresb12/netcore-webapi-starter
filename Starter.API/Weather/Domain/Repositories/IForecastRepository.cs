using Starter.API.Weather.Domain.Models;

namespace Starter.API.Weather.Domain.Repositories;

public interface IForecastRepository {
  Task<IEnumerable<Forecast>> ListAll();

  Task Add(Forecast forecast);

  Task<Forecast?> FindById(long id);

  void Update(Forecast forecast);

  void Remove(Forecast forecast);
}
