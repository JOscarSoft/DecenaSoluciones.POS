using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using DecenaSoluciones.POS.App.Extensions;
using CurrieTechnologies.Razor.SweetAlert2;
using DecenaSoluciones.POS.Shared.Services;
using DecenaSoluciones.POS.Shared.Extensions;

namespace DecenaSoluciones.POS.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            
            builder.Services.AddTransient<JwtTokenHeaderHandler>();
            builder.Services.AddScoped<ILocalStorage, LocalStorageExtension>();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthExtension>();
            builder.Services.AddHttpClient("WebApi", client => client.BaseAddress = new Uri("https://webapi.decenasoluciones.com/"))
                .AddHttpMessageHandler<JwtTokenHeaderHandler>();
            builder.Services.AddHttpClient("Local", client => client.BaseAddress = new Uri("http://localhost:5212/"));
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ISaleService, SaleService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IResourceService, ResourceService>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddSweetAlert2();

            return builder.Build();
        }
    }
}
