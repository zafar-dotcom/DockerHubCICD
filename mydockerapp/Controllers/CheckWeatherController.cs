using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace mydockerapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestJokeController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public TestJokeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("random")]
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

        public class JokeResponse
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Setup { get; set; }
            public string Punchline { get; set; }
        }
    }
}
