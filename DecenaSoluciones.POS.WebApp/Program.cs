using BlazorBootstrap;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using DecenaSoluciones.POS.WebApp;
using DecenaSoluciones.POS.WebApp.Extensions;
using DecenaSoluciones.POS.WebApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage(); 
builder.Services.AddTransient<JwtTokenHeaderHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthExtension>();
builder.Services.AddHttpClient("WebApi", client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl") ?? builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<JwtTokenHeaderHandler>();
builder.Services.AddHttpClient("Local", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
