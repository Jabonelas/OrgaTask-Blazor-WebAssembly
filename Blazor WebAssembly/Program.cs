using Blazor_WebAssembly.Extensions;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Blazor_WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#if DEBUG

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7091/") });
#else

            builder.Services.AddScoped(sp =>
        new HttpClient
        {
            BaseAddress = new Uri(builder.Configuration["https://blazor-api.onrender.com"] ?? "https://blazor-api.onrender.com")
        });

#endif

            //Adicionando as injeções de dependencias
            builder.Services.AdicionarInjecoesDependencias();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}