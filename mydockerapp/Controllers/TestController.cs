using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

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

        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                var healthData = new
                {
                    Status = "Healthy",
                    Timestamp = DateTime.UtcNow,
                    Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    MachineName = Environment.MachineName,
                    OSVersion = Environment.OSVersion.ToString(),
                    ProcessorCount = Environment.ProcessorCount,
                    WorkingSet = GC.GetTotalMemory(false),
                    Uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()
                };

                return Ok(healthData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Unhealthy", Error = ex.Message });
            }
        }

        [HttpGet("system-info")]
        public async Task<IActionResult> GetSystemInfo()
        {
            try
            {
                var systemInfo = new
                {
                    Server = new
                    {
                        MachineName = Environment.MachineName,
                        OSVersion = Environment.OSVersion.ToString(),
                        ProcessorCount = Environment.ProcessorCount,
                        Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                        Is64BitProcess = Environment.Is64BitProcess,
                        SystemDirectory = Environment.SystemDirectory,
                        CurrentDirectory = Environment.CurrentDirectory
                    },
                    Application = new
                    {
                        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                        ProcessId = Environment.ProcessId,
                        StartTime = Process.GetCurrentProcess().StartTime,
                        WorkingSet = Process.GetCurrentProcess().WorkingSet64,
                        PrivateMemorySize = Process.GetCurrentProcess().PrivateMemorySize64
                    },
                    Runtime = new
                    {
                        DotNetVersion = Environment.Version.ToString(),
                        RuntimeIdentifier = System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier,
                        Framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription
                    }
                };

                return Ok(systemInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("simulate-task")]
        public async Task<IActionResult> SimulateTask([FromBody] TaskRequest request)
        {
            try
            {
                var taskId = Guid.NewGuid().ToString();
                var startTime = DateTime.UtcNow;

                // Simulate some work
                await Task.Delay(request.DelayMs > 0 ? request.DelayMs : 1000);

                var result = new
                {
                    TaskId = taskId,
                    TaskName = request.TaskName ?? "Default Task",
                    Status = "Completed",
                    StartTime = startTime,
                    EndTime = DateTime.UtcNow,
                    Duration = DateTime.UtcNow - startTime,
                    Result = $"Task '{request.TaskName}' completed successfully after {request.DelayMs}ms delay"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("random-data")]
        public async Task<IActionResult> GetRandomData()
        {
            var random = new Random();
            var data = new
            {
                Id = Guid.NewGuid(),
                RandomNumber = random.Next(1, 1000),
                RandomDecimal = Math.Round(random.NextDouble() * 100, 2),
                RandomBoolean = random.Next(2) == 1,
                RandomString = GenerateRandomString(10),
                Timestamp = DateTime.UtcNow,
                RandomArray = Enumerable.Range(1, 5).Select(x => random.Next(1, 100)).ToArray()
            };

            return Ok(data);
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public class TaskRequest
        {
            public string? TaskName { get; set; }
            public int DelayMs { get; set; } = 1000;
        }
    }
}
