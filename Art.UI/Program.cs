using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Art.UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(
    sp =>
    {
        var settings = new Settings();
        settings.Init("/SampleImages/");
        return settings;
    });

builder.Services.AddScoped<IApiManager, BogusApiManager>();

await builder.Build().RunAsync();
