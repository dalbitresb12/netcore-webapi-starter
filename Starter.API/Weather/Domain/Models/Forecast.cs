namespace Starter.API.Weather.Domain.Models;

public class Forecast {
  public long Id { get; set; }
  public DateTime Date { get; set; }
  public int TemperatureC { get; set; }
  public string? Summary { get; set; }
}
