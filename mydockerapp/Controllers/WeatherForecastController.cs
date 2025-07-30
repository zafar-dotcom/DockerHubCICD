using Microsoft.AspNetCore.Mvc;

namespace mydockerapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string[] Cities = new[]
        {
            "New York", "London", "Paris", "Tokyo", "Sydney", "Toronto", "Berlin", "Madrid", "Rome", "Amsterdam"
        };

        public WeatherForecastController()
        {
            
        }

        [HttpGet(Name = "TestDeployement")]
        public async Task<IActionResult> Get()
        {
            return Ok(new { status = "Deployement on Azure web services " });
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetWeatherForecast()
        {
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            return Ok(forecasts);
        }

        [HttpGet("forecast/{days:int}")]
        public async Task<IActionResult> GetExtendedForecast(int days)
        {
            if (days < 1 || days > 30)
            {
                return BadRequest("Days must be between 1 and 30");
            }

            var forecasts = Enumerable.Range(1, days).Select(index => new DetailedWeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                TemperatureF = 32 + (int)(Random.Shared.Next(-20, 55) / 0.5556),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Humidity = Random.Shared.Next(20, 90),
                WindSpeed = Math.Round(Random.Shared.NextDouble() * 20, 1),
                Pressure = Random.Shared.Next(990, 1030),
                ChanceOfRain = Random.Shared.Next(0, 100)
            }).ToArray();

            return Ok(new
            {
                RequestedDays = days,
                GeneratedDate = DateTime.UtcNow,
                Forecasts = forecasts
            });
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetWeatherForMultipleCities()
        {
            var cityWeather = Cities.Take(5).Select(city => new CityWeather
            {
                City = city,
                Country = GetRandomCountry(),
                Temperature = Random.Shared.Next(-10, 40),
                Description = Summaries[Random.Shared.Next(Summaries.Length)],
                Humidity = Random.Shared.Next(30, 90),
                LastUpdated = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(0, 60))
            }).ToArray();

            return Ok(new
            {
                TotalCities = cityWeather.Length,
                LastRefresh = DateTime.UtcNow,
                Cities = cityWeather
            });
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeather()
        {
            var currentWeather = new CurrentWeather
            {
                Location = "Current Location",
                Temperature = Random.Shared.Next(15, 30),
                FeelsLike = Random.Shared.Next(15, 35),
                Description = Summaries[Random.Shared.Next(Summaries.Length)],
                Humidity = Random.Shared.Next(40, 80),
                Pressure = Random.Shared.Next(990, 1030),
                WindSpeed = Math.Round(Random.Shared.NextDouble() * 15, 1),
                WindDirection = GetRandomWindDirection(),
                Visibility = Random.Shared.Next(5, 15),
                UvIndex = Random.Shared.Next(1, 10),
                Sunrise = DateTime.Today.AddHours(6).AddMinutes(Random.Shared.Next(0, 60)),
                Sunset = DateTime.Today.AddHours(18).AddMinutes(Random.Shared.Next(0, 60)),
                Timestamp = DateTime.UtcNow
            };

            return Ok(currentWeather);
        }

        private string GetRandomCountry()
        {
            var countries = new[] { "USA", "UK", "France", "Japan", "Australia", "Canada", "Germany", "Spain", "Italy", "Netherlands" };
            return countries[Random.Shared.Next(countries.Length)];
        }

        private string GetRandomWindDirection()
        {
            var directions = new[] { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
            return directions[Random.Shared.Next(directions.Length)];
        }

        public class WeatherForecast
        {
            public DateOnly Date { get; set; }
            public int TemperatureC { get; set; }
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
            public string? Summary { get; set; }
        }

        public class DetailedWeatherForecast : WeatherForecast
        {
            public int Humidity { get; set; }
            public double WindSpeed { get; set; }
            public int Pressure { get; set; }
            public int ChanceOfRain { get; set; }
        }

        public class CityWeather
        {
            public string City { get; set; } = string.Empty;
            public string Country { get; set; } = string.Empty;
            public int Temperature { get; set; }
            public string Description { get; set; } = string.Empty;
            public int Humidity { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public class CurrentWeather
        {
            public string Location { get; set; } = string.Empty;
            public int Temperature { get; set; }
            public int FeelsLike { get; set; }
            public string Description { get; set; } = string.Empty;
            public int Humidity { get; set; }
            public int Pressure { get; set; }
            public double WindSpeed { get; set; }
            public string WindDirection { get; set; } = string.Empty;
            public int Visibility { get; set; }
            public int UvIndex { get; set; }
            public DateTime Sunrise { get; set; }
            public DateTime Sunset { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}
