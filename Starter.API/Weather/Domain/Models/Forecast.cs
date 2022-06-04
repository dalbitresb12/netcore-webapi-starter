using Starter.API.Shared.Domain.Model;

namespace Starter.API.Weather.Domain.Models;

public class Forecast : BaseModel {
  public DateTime Date { get; set; }
  public int TemperatureC { get; set; }
  public string? Summary { get; set; }
}
