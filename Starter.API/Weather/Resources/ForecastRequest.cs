namespace Starter.API.Weather.Resources;

public class ForecastRequest {
  public DateTime Date { get; set; }
  public int TemperatureC { get; set; }
  public string? Summary { get; set; }
}
