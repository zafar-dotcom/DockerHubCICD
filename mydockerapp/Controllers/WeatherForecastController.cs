using Microsoft.AspNetCore.Mvc;

namespace mydockerapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       


        public WeatherForecastController()
        {
            
        }

        [HttpGet(Name = "TestDeployement")]
        public async Task<IActionResult> Get()
        {
            return Ok(new { status = "Deployement on Azure web services" });
        }
    }
}
