using Blazor_WebAssembly.Interfaces.Tarefa;
using Blazor_WebAssembly.Interfaces.Usuario;
using Blazor_WebAssembly.Services.Tarefa;
using Blazor_WebAssembly.Services.Usuario;
using Blazor_WebAssembly.ViewModel.Tarefa;
using Blazor_WebAssembly.ViewModel.Usuario;


namespace Blazor_WebAssembly.Extensions
{
    public static class InjecaoDependencia
    {
        public static IServiceCollection AdicionarInjecoesDependencias(this IServiceCollection service)
        {
            //Usuario
            service.AddScoped<IUsuarioService, UsuarioService>();

            //Tarefa
            service.AddScoped<ITarefaService, TarefaService>();

            //View Model
            service.AddScoped<LoginViewModel>();
            service.AddScoped<CadastrarUsuarioViewModel>();
            service.AddScoped<VisualizarTarefaViewModel>();
            service.AddScoped<AlterarTarefaViewModel>();
            service.AddScoped<CadastrarTarefaViewModel>();
            service.AddScoped<DashboardTarefasViewModel>();
            service.AddScoped<TarefaRolagemViewModel>();

            //Notivicacao
            service.AddScoped<NotificacaoService>();

            return service;
        }
    }
}