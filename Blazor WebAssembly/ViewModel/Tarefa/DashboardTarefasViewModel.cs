using Blazor_WebAssembly.DTOs.Tarefa;
using Blazor_WebAssembly.Interfaces.Tarefa;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor_WebAssembly.ViewModel.Tarefa
{
    public partial class DashboardTarefasViewModel : ObservableObject
    {

        private readonly ITarefaService iTarefaService;
        private readonly NotificacaoService notificacaoService;
        private readonly NavigationManager navigation;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        [ObservableProperty]
        public int pendenteCount = 0;

        [ObservableProperty]
        public int emProgressoCount = 0;

        [ObservableProperty]
        public int concluidoCount = 0;

        [ObservableProperty]
        public decimal porcentagemTarefasConcluidas = 0;

        [ObservableProperty]

        public List<TarefaPrioridadeAltaDTO> listaTarefasPrioridadeAlta = new List<TarefaPrioridadeAltaDTO>();


        public DashboardTarefasViewModel(
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


        public async Task CarregarInformacoes()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
            {
                await notificacaoService.MostrarErro("Sessão expirada. Por favor, faça login novamente.");

                navigation.NavigateTo("/login");
                //navigation.NavigateTo("/", forceLoad: true);

            }

            await CarregarInformacoesStatusAsync();

            await CarregarTarefasPrioridadeAltaAsync();
        }

        public async Task CarregarInformacoesStatusAsync()
        {
            try
            {
                var (success, errorMessage, TarefaQtdStatus) = await iTarefaService.BuscarQtdStatusTarefaAsync();

                if (success)
                {
                    pendenteCount = TarefaQtdStatus.Pendente;
                    emProgressoCount = TarefaQtdStatus.EmProgresso;
                    concluidoCount = TarefaQtdStatus.Concluido;
                    porcentagemTarefasConcluidas = TarefaQtdStatus.PorcentagemConcluidas;
                }
                else
                {
                    await notificacaoService.MostrarErro(errorMessage);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao buscar quantidade status tarefa: {ex.Message}");

                await notificacaoService.MostrarErro($"Ocorreu um erro interno. Nossa equipe já foi notificada.");
            }
        }

        public async Task CarregarTarefasPrioridadeAltaAsync()
        {
            try
            {
                var (success, errorMessage, listaTarefaPrioridadeAlta) = await iTarefaService.BuscarTarefasPrioridadeAltaAsync();

                if (success && listaTarefaPrioridadeAlta != null)
                {
                    listaTarefasPrioridadeAlta = listaTarefaPrioridadeAlta;
                }
                else
                {
                    await notificacaoService.MostrarErro(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar porcentagem de tarefas concluidas: {ex.Message}");

                await notificacaoService.MostrarErro($"Ocorreu um erro interno. Nossa equipe já foi notificada.");
            }
        }

        public void TarefasPendentes(string status)
        {

            navigation.NavigateTo($"/tarefa-rolagem/{status}");
        }
    }
}
