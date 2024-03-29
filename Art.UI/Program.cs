global using Art.Core;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Art.UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddTransient<ILocalStorage, LocalStorage>();
builder.Services.AddTransient<ILikeService, LikeService>();
builder.Services.AddTransient<IClipboardService, ClipboardService>();
builder.Services.AddTransient<IHistoryService, HistoryService>();
builder.Services.AddTransient<IDataAccessService, DataAccessService>();

var app = builder.Build();

// Initialize dependency injection container
DI.Init(app.Services);

await app.RunAsync();