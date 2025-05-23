using Blazor_WebAssembly.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazor_WebAssembly.Layout;
using Blazor_WebAssembly.Pages;
using Blazor_WebAssembly.Services.Usuario;
using Blazor_WebAssembly.Services.Tarefa;
using Blazor_WebAssembly.Interfaces.Tarefa;

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

                   builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7091/api/") });
#else

            builder.Services.AddScoped(sp =>
        new HttpClient
        {
            BaseAddress = new Uri(builder.Configuration["https://blazor-api.onrender.com"] ?? "https://blazor-api.onrender.com")
        });

#endif

            // tarefa
            builder.Services.AddScoped<ITarefaService, TarefaService>();

            // usuario
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();

            // injetando notivicacao
            builder.Services.AddScoped<NotificacaoService>();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}