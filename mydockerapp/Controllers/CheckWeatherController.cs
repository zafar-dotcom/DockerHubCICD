using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace mydockerapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestWeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "7ecb6362c58a8f26dd6a1d5d1b6c1cd1";

        public TestWeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetWeatherForCity(string city)
        {
           // string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}&units=metric";
            string url= $"https://api.openweathermap.org/data/3.0/onecall?lat=31.5204&lon=74.3587&appid={ApiKey}&units=metric";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch weather data");
            }

            string content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonConvert.DeserializeObject<WeatherApiResponse>(content);

            var result = new
            {
                City = weatherData.Name,
                Temperature = weatherData.Main.Temp,
                Condition = weatherData.Weather.FirstOrDefault()?.Main,
                Description = weatherData.Weather.FirstOrDefault()?.Description
            };

            return Ok(result);
        }
        public class WeatherMain
        {
            public float Temp { get; set; }
        }

        public class WeatherDescription
        {
            public string Main { get; set; }
            public string Description { get; set; }
        }

        public class WeatherApiResponse
        {
            public WeatherMain Main { get; set; }
            public List<WeatherDescription> Weather { get; set; }
            public string Name { get; set; }
        }

    }
}