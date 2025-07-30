using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace mydockerapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckWeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CheckWeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("random-joke")]
        public async Task<IActionResult> GetRandomJoke()
        {
            string url = "https://official-joke-api.appspot.com/jokes/random";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch joke");
            }

            string content = await response.Content.ReadAsStringAsync();
            var joke = JsonConvert.DeserializeObject<JokeResponse>(content);

            return Ok(new
            {
                Setup = joke.Setup,
                Punchline = joke.Punchline
            });
        }

        [HttpGet("weather/{city}")]
        public async Task<IActionResult> GetWeatherByCity(string city)
        {
            try
            {
                // Using OpenWeatherMap API (free tier) - you'd need to register for an API key
                // For demo purposes, I'll simulate weather data
                var weatherData = new
                {
                    City = city,
                    Temperature = new Random().Next(-10, 35), // Random temp between -10 and 35°C
                    Description = GetRandomWeatherDescription(),
                    Humidity = new Random().Next(20, 90),
                    WindSpeed = Math.Round(new Random().NextDouble() * 20, 1),
                    Timestamp = DateTime.UtcNow
                };

                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching weather data: {ex.Message}");
            }
        }

        [HttpGet("weather-current")]
        public async Task<IActionResult> GetCurrentWeather()
        {
            try
            {
                var currentWeather = new
                {
                    Location = "Default Location",
                    Temperature = new Random().Next(15, 30),
                    Description = GetRandomWeatherDescription(),
                    FeelsLike = new Random().Next(15, 35),
                    Humidity = new Random().Next(40, 80),
                    Pressure = new Random().Next(990, 1030),
                    Visibility = new Random().Next(5, 15),
                    UvIndex = new Random().Next(1, 10),
                    Timestamp = DateTime.UtcNow,
                    Forecast = GenerateWeeklyForecast()
                };

                return Ok(currentWeather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching current weather: {ex.Message}");
            }
        }

        private string GetRandomWeatherDescription()
        {
            var descriptions = new[]
            {
                "Sunny", "Partly Cloudy", "Cloudy", "Overcast", "Light Rain", 
                "Heavy Rain", "Thunderstorm", "Snow", "Foggy", "Windy"
            };
            return descriptions[new Random().Next(descriptions.Length)];
        }

        private object[] GenerateWeeklyForecast()
        {
            var forecast = new List<object>();
            for (int i = 1; i <= 7; i++)
            {
                forecast.Add(new
                {
                    Day = DateTime.Today.AddDays(i).DayOfWeek.ToString(),
                    Date = DateTime.Today.AddDays(i).ToString("yyyy-MM-dd"),
                    HighTemp = new Random().Next(20, 35),
                    LowTemp = new Random().Next(5, 20),
                    Description = GetRandomWeatherDescription()
                });
            }
            return forecast.ToArray();
        }

        public class JokeResponse
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Setup { get; set; }
            public string Punchline { get; set; }
        }
    }
}
