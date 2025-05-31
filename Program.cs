using Scalar.AspNetCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/fill-form", async () =>
{
    using var driver = new ChromeDriver();
    try
    {
        driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");
        await Task.Delay(1000);

        SmartFormFiller.FillAllNameFields(driver);

        Console.WriteLine("All fields filled. Press ENTER to exit...");
        Console.ReadLine();
        return Results.Ok("All form fields filled.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error: {ex.Message}");
    }
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
