using System.Threading.Tasks;
using Microsoft.Playwright;

class Program
{
    public static async Task Main()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });

        var context = await browser.NewContextAsync(new()
        {
            Permissions = new[] { "geolocation" },
            Geolocation = new Geolocation() { Longitude = 49.99175855135149f, Latitude = 17.95974936453222f }
        });

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://wp2.pvforecast.cz/predpoved-osvitu/");

        var data = new { lat = 49.982992480271285, lng = 18.008260947659032}; // Oldrisov
        // Pass data as a parameter
        var result = await page.EvaluateAsync("data => {" +
            "window.mark.setLatLng(data);" +
            "window.mark.bindPopup(data);" +
            "window.graf_data(window.data_storage, data);" +
            "}", data);

        //await page.Mouse.ClickAsync(1120f, 450f);
        await page.ScreenshotAsync(new() { Path = "predpoved.png" });
    }
}
