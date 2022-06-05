using System.ComponentModel.DataAnnotations;

namespace Starter.API.Weather.Resources;

public class ForecastRequest {
  [Required]
  public DateTime? Date { get; set; }
  [Required]
  public int? TemperatureC { get; set; }
  public string? Summary { get; set; }
}
