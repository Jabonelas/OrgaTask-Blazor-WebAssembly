using Blazor_WebAssembly.DTOs.Tarefa;
using Blazor_WebAssembly.Interfaces.Tarefa;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Blazor_WebAssembly.ViewModel.Tarefa
{
    public partial class TarefaRolagemViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;
        private readonly NotificacaoService notificacaoService;
        private readonly NavigationManager navigation;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        [ObservableProperty]
        public List<TarefaConsultaDTO> tarefas = new();

        [ObservableProperty]
        public string titulo = "";

        [ObservableProperty]
        public string status = "";

        public const int pageSize = 15;

        [ObservableProperty]
        public int currentPage = 1;

        [ObservableProperty]
        public int totalCount = 0;

        [ObservableProperty]
        public bool isLoading = false;

        [ObservableProperty]
        public bool hasMoreItems = true;

        [ObservableProperty]
        public bool isProcessing = false;

        public event Action? OnStateChanged;

        public TarefaRolagemViewModel(
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

        private void NotifyStateChanged() => OnStateChanged?.Invoke();

        public async Task VertificarSecaoValidaAsync(string _status)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
            {
                await notificacaoService.MostrarErro("Sessão expirada. Por favor, faça login novamente.");
                navigation.NavigateTo("/login");
            }

            status = _status;
        }

        [JSInvokable]
        public async Task CarregandoMaisItensAsync()
        {
            if (isProcessing || isLoading || !hasMoreItems) return;

            isProcessing = true;
            isLoading = true;

            NotifyStateChanged();

            try
            {
                var (success, errorMessage, items, newTotalCount) =
                    await iTarefaService.ObterTarefasPaginadasAsync(currentPage, pageSize, status);

                Console.WriteLine($"API Response - Success: {success}, Count: {items?.Count}, Total: {newTotalCount}");

                if (success)
                {
                    SentandoTituloPagina();

                    if (items?.Count > 0)
                    {
                        tarefas.AddRange(items);
                        currentPage++;
                        totalCount = newTotalCount;
                        hasMoreItems = tarefas.Count < totalCount;
                    }
                    else
                    {
                        hasMoreItems = false;
                    }
                }
                else
                {
                    await notificacaoService.MostrarErro(errorMessage ?? "Erro ao carregar tarefas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar itens: {ex.Message}");
                await notificacaoService.MostrarErro($"Erro ao carregar mais itens: {ex.Message}");
            }
            finally
            {
                isProcessing = false;
                isLoading = false;
                NotifyStateChanged();
            }
        }

        public void SentandoTituloPagina()
        {
            switch (status)
            {
                case "todas": titulo = "Todas as Minhas Tarefas"; break;
                case "Pendente": titulo = "Todas as Minhas Tarefas Pendentes"; break;
                case "Em Progresso": titulo = "Todas as Minhas Tarefas Em Progresso"; break;
                case "Concluído": titulo = "Todas as Minhas Tarefas Concluídas"; break;
                default: titulo = "Minhas Tarefas"; break;
            }

            NotifyStateChanged();
        }

        public async Task ResetandoPaginacaoAsync()
        {
            currentPage = 1;
            tarefas.Clear();
            hasMoreItems = true;
            NotifyStateChanged();

            await Task.Delay(10);
            await CarregandoMaisItensAsync();
        }

        public string PegandoIconePrioridade(string prioridade)
        {
            switch (prioridade)
            {
                case "Alta": return "bi-exclamation-triangle-fill";
                case "Média": return "bi-exclamation-circle-fill";
                case "Baixa": return "bi-arrow-down-circle-fill";
            }
            return "bi-question-circle";
        }

        public string PegandoIconeStatus(string status)
        {
            switch (status)
            {
                case "Concluído": return "bi-check-circle-fill";
                case "Em Progresso": return "bi-arrow-repeat";
                case "Pendente": return "bi-clock";
            }
            return "bi-question-circle";
        }

        public string PegandoClassPrioridade(string prioridade)
        {
            switch (prioridade)
            {
                case "Alta": return "prioridade-alta";
                case "Média": return "prioridade-media";
                case "Baixa": return "prioridade-baixa";
            }
            return "";
        }

        public string PegandoClassStatus(string status)
        {
            switch (status)
            {
                case "Concluído": return "status-concluido";
                case "Em Progresso": return "status-progresso";
                case "Pendente": return "status-pendente";
            }
            return "";
        }

        public string FormatandoData(object dateObj)
        {
            if (dateObj == null) return "-";
            if (dateObj is DateTime date) return date.ToString("dd/MM/yyyy");
            if (DateTime.TryParse(dateObj.ToString(), out var parsedDate)) return parsedDate.ToString("dd/MM/yyyy");
            return dateObj.ToString();
        }

        public void VisualizarTarefa(int id)
        {
            navigation.NavigateTo($"/exibir-tarefa/{id}");
        }

        public void EditarTarefa(int id)
        {
            navigation.NavigateTo($"/editar-tarefa/{id}");
        }

        public void NovaTarefa()
        {
            navigation.NavigateTo("/cadastrar-tarefa");
        }

        public async Task ConfirmarExclusaoAsync(int id)
        {
            bool confirmado = await notificacaoService.MostrarConfirmacao("Atenção!", "Tem certeza que deseja excluir esta tarefa?");
            if (confirmado)
            {
                try
                {
                    await iTarefaService.DeletarTarefaAsync(id);
                    await ResetandoPaginacaoAsync();
                    await notificacaoService.MostrarSucesso("Tarefa excluída com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao excluir tarefa: {ex.Message}");
                    await notificacaoService.MostrarErro("Ocorreu um erro ao excluir a tarefa.");
                }
            }
        }
    }
}
