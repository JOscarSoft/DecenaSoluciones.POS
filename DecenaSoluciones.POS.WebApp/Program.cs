using BlazorBootstrap;
using CurrieTechnologies.Razor.SweetAlert2;
using DecenaSoluciones.POS.WebApp;
using DecenaSoluciones.POS.WebApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl"))});
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
