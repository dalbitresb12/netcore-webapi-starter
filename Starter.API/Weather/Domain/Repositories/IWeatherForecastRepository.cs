namespace Starter.API.Weather.Domain.Repositories;

public interface IWeatherForecastRepository {
  Task<IEnumerable<WeatherForecast>> ListAll();
  Task Add(WeatherForecast forecast);
  Task<WeatherForecast> FindById(int id);
  Task Update(WeatherForecast forecast);
  Task Remove(WeatherForecast forecast);
}
