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

        public List<TarefaPrioridadeAlta> listaTarefasPrioridadeAlta = new List<TarefaPrioridadeAlta>();


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
                var (Sucesso, ErrorMessagem, TarefaQtdStatus) = await iTarefaService.BuscarQtdStatusTarefaAsync();

                if (Sucesso)
                {
                    pendenteCount = TarefaQtdStatus.Pendente;
                    emProgressoCount = TarefaQtdStatus.EmProgresso;
                    concluidoCount = TarefaQtdStatus.Concluido;
                    porcentagemTarefasConcluidas = TarefaQtdStatus.PorcentagemConcluidas;

                }
                else
                {
                    await notificacaoService.MostrarErro(ErrorMessagem);
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
                var (Sucesso, ErrorMessagem, ListaTarefaPrioridadeAlta) = await iTarefaService.BuscarTarefasPrioridadeAltaAsync();

                if (Sucesso && ListaTarefaPrioridadeAlta != null)
                {

                    listaTarefasPrioridadeAlta.Clear();

                    string descricaoPrazo = string.Empty;

                    foreach (var tarefa in ListaTarefaPrioridadeAlta)
                    {
                        descricaoPrazo = tarefa.Prazo < 0 ? $"Prazo: {tarefa.Prazo.ToString().Replace("-", "")} dia(s) em atraso." : $"Prazo:{tarefa.Prazo} dia(s) restante(s).";

                        listaTarefasPrioridadeAlta.Add(new TarefaPrioridadeAlta
                        {
                            Titulo = tarefa.Titulo,
                            Data = tarefa.Data,
                            Prazo = tarefa.Prazo,
                            DescricaoPrazo = descricaoPrazo,
                            Status = tarefa.Status
                        });
                    }
                }
                else
                {
                    await notificacaoService.MostrarErro(ErrorMessagem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar porcentagem de tarefas concluidas: {ex.Message}");

                await notificacaoService.MostrarErro($"Ocorreu um erro interno. Nossa equipe já foi notificada.");
            }
        }

        public class TarefaPrioridadeAlta
        {
            public string Titulo { get; set; }
            public string Data { get; set; }
            public int Prazo { get; set; }
            public string DescricaoPrazo { get; set; }
            public string Status { get; set; }

        }

        public void TarefasPendentes(string status)
        {
            navigation.NavigateTo($"/tarefa-rolagem/{status}");
        }
    }
}
