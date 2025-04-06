using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProductInventory.Blazor;
using ProductInventory.Blazor.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

var host = builder.Configuration["Host"];
var baseAddress = string.IsNullOrEmpty(host) ? builder.HostEnvironment.BaseAddress : host;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddSingleton<ProductFilterState>();
builder.Services.AddSingleton<AppStateService>();

await builder.Build().RunAsync();
