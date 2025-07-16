var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// ? Only bind to port 80 when in Docker
if (isDocker)
{
   // Add services to the container.
builder.WebHost.UseUrls("http://*:80");
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
app.UseSwaggerUI();
//}
if (isDocker)
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // ?? makes Swagger UI the homepage
    });
    app.MapGet("/", () => "Hello from Docker!");
}

app.UseAuthorization();

app.MapControllers();

app.Run();
