using BlazorBootstrap;
using CurrieTechnologies.Razor.SweetAlert2;
using DecenaSoluciones.POS.WebApp;
using DecenaSoluciones.POS.WebApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl") ?? builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("WebApi", client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl") ?? builder.HostEnvironment.BaseAddress));
builder.Services.AddHttpClient("Local", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
