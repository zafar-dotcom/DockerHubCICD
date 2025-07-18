var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

// Check if running inside Docker
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// Bind to port 80 when in Docker
if (isDocker)
{
    builder.WebHost.UseUrls("http://*:80");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development or Docker
if (app.Environment.IsDevelopment() || isDocker)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = isDocker ? string.Empty : "swagger"; // In Docker, Swagger is homepage
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
