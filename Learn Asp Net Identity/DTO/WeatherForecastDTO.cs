namespace Learn_Asp_Net_Identity.DTO;

public class WeatherForecastDTO
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF { get; set; }// => 32 + (int)(TemperatureC / 0.5556);
    public string Summary { get; set; }
}