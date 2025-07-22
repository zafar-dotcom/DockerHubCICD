var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

// Determine environment
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

// Only override port when in container
if (isDocker)
{
    builder.WebHost.UseUrls($"http://*:{port}");
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
        c.RoutePrefix = string.Empty; // Makes Swagger UI load at root
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
