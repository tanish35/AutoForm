using Scalar.AspNetCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.Json.Serialization;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


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
    var chromeOptions = new ChromeOptions();
    // chromeOptions.AddArgument("--headless");

    using var driver = new ChromeDriver(chromeOptions);
    try
    {
        driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");

        // var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        // wait.Until(drv => drv.FindElements(By.TagName("iframe")).Count > 2);

        await Task.Delay(5000);

        var nameField = SmartFormFiller.FindNameField(driver);

        if (nameField is not null)
        {
            nameField.SendKeys("Tanish Majumdar");
            Console.WriteLine("Form filled. Press ENTER to exit and close the browser...");
            Console.ReadLine();
            return Results.Ok("Form field filled.");
        }
        else
        {
            return Results.Problem("Name field could not be detected.");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error occurred: {ex.Message}");
    }
})
.WithName("FillAutomationForm");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
