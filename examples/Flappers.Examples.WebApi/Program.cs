using Flappers.Pipeline;
using Flappers.TryCatch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", TryCatch(Pipeline(() =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})))
.WithName("GetWeatherForecast");

app.Run();

static Func<WeatherForecast[]> TryCatch(Func<WeatherForecast[]> handler)
{
    return handler
        .Catch<Exception, WeatherForecast[]>(ex =>
         {
             Console.WriteLine(ex.Message);
             return Array.Empty<WeatherForecast>();
         })
        .Execute;
}

static Func<WeatherForecast[]> Pipeline(Func<WeatherForecast[]> value)
{
    return value
        .AddStage((next) =>
        {
            Console.WriteLine("Before");
            var result = next();
            Console.WriteLine("After");
            return result;
        })
        .Execute;
}

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}