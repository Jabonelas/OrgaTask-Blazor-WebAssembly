using Blazor_WebAssembly.DTOs.Tarefa;
using Blazor_WebAssembly.Interfaces.Tarefa;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor_WebAssembly.ViewModel.Tarefa
{
    public partial class VisualizarTarefaViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;
        private readonly NotificacaoService notificacaoService;
        private readonly NavigationManager navigation;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        [ObservableProperty]
        public TarefaAlterarDTO tarefaAlterarDTO = new TarefaAlterarDTO();

        public VisualizarTarefaViewModel(
            ITarefaService _iTarefaService,
            NotificacaoService _notificacaoService,
            NavigationManager _navigation,
            AuthenticationStateProvider _authenticationStateProvider)
        {
            iTarefaService = _iTarefaService;
            notificacaoService = _notificacaoService;
            navigation = _navigation;
            authenticationStateProvider = _authenticationStateProvider;
        }

        public async Task BuscarTarefaAsync(int _id)
        {
            try
            {
                var authState = await authenticationStateProvider.GetAuthenticationStateAsync();

                var user = authState.User;

                if (!user.Identity.IsAuthenticated)
                {
                    await notificacaoService.MostrarErro("Sessão expirada. Por favor, faça login novamente.");

                    navigation.NavigateTo("/login");
                }

                (bool Sucesso, string ErrorMessagem, TarefaAlterarDTO tarefa) = await iTarefaService.BuscarTarefaAsync(_id);

                if (Sucesso)
                {
                    TarefaAlterarDTO = tarefa;

                }
                else
                {
                    await notificacaoService.MostrarErro(ErrorMessagem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao visualizar tarefas: {ex.Message}");

                await notificacaoService.MostrarErro($"Ocorreu um erro interno. Nossa equipe já foi notificada.");
            }
        }

        public void VoltarPaginaTarefas()
        {
            navigation.NavigateTo($"/tarefa-rolagem/todas");
        }

    }
}
