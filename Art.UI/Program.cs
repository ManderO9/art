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

builder.Services.AddScoped<IImagesService, BogusImagesService>();
builder.Services.AddTransient<ILocalStorage, LocalStorage>();
builder.Services.AddTransient<ILikeService, LikeService>();
builder.Services.AddTransient<IClipboardService, ClipboardService>();
builder.Services.AddTransient<IHistoryService, HistoryService>();

var app = builder.Build();

// Initialize dependency inject container
DI.Init(app.Services);

await app.RunAsync();

// TODO: add time on top of pictures, maybe that will look good
// TODO: add image page where you can see details about an image
// TODO: add history page where you will see all the images that you saw in order
// TODO: add something that will add images to the history automatically


