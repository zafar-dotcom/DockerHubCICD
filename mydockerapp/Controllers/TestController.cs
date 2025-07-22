using Microsoft.AspNetCore.Mvc;

namespace mydockerapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
       


        public TestController()
        {
            
        }

        [HttpGet(Name = "TestAzureDeployement")]
        public async Task<IActionResult> Get()
        {
            return Ok(new { status = "Deployement on Azure web services via github repo" });
        }
    }
}
