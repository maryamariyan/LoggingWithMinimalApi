using Demo;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders().AddConsole().AddCustomFormatter(o =>
{
    o.CustomPrefix = Environment.NewLine + " >>> ";
    o.ColorBehavior = LoggerColorBehavior.Default;
});

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
var residents = new[]
{
    "Sam", "Tina", "Chester", "Brian"
};

app.MapGet("/", () =>
{
    Log.HelloWorld(app.Logger);
})
.WithName("Index");

app.MapGet("/weatherforecast", () =>
{
    var forecasts = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    foreach (WeatherForecast forecastOnDay in forecasts)
    {
        Log.GotWeatherForecastWithSummary(app.Logger, forecastOnDay.Summary!);
    }

    return forecasts;
})
.WithName("GetWeatherForecast");

app.MapGet("/residentinfo", () =>
{
    var resident = new Resident
    (
        residents[Random.Shared.Next(residents.Length)],
        Random.Shared.Next(19, 55),
        "Vancouver",
        Random.Shared.Next(1, 11),
        2
    );

    // fast to write, slow to execute
    app.Logger.LogInformation(1010, 
        "Resident {Name} aged {Age} years old from {Hometown} moved here {YearsSince} years with {NumSuitcases} suitcases!",
        resident.Name, resident.Age, resident.Hometown, resident.YearsSince, resident.NumSuitcases);

    // slow to write, faster to execute
    Log.NewResidentInformationUserDefined(app.Logger, resident.Name, resident.Age, resident.Hometown, resident.YearsSince, resident.NumSuitcases);

    // NEW: fast to write and to execute
    Log.NewResidentInformationGenerated(app.Logger, resident.Name, resident.Age, resident.Hometown, resident.YearsSince, resident.NumSuitcases);
})
.WithName("GetResidentInformantion");

#region SkipEnabledCheckFeature
app.MapGet("/skipenabledcheckfalse", () =>
{
    CheckElapsedTime(() =>
    {
        for (int i = 0; i < 100_000; i++)
            Log.SkipEnabledCheckFalse(app.Logger);
    });
})
.WithName("SkipEnabledCheckFalse");

app.MapGet("/skipenabledchecktrue", () =>
{
    CheckElapsedTime(() =>
    {
        for (int i = 0; i < 100_000; i++)
            Log.SkipEnabledCheckTrue(app.Logger);
    });
})
.WithName("SkipEnabledCheckTrue");
#endregion

app.Run();

void CheckElapsedTime(Action action)
{
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    action();
    stopWatch.Stop();
    // Get the elapsed time as a TimeSpan value.
    TimeSpan ts = stopWatch.Elapsed;

    // Format and display the TimeSpan value.
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);

    Log.LoggedInElapsedTime(app.Logger, elapsedTime);
}

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record Resident(string Name, int Age, string Hometown, int YearsSince, long NumSuitcases) { }
