using Blazor_WebAssembly.DTOs.Tarefa;
using Blazor_WebAssembly.Interfaces.Tarefa;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor_WebAssembly.ViewModel.Tarefa
{
    public partial class CadastrarTarefaViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;
        private readonly NotificacaoService notificacaoService;
        private readonly NavigationManager navigation;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        [ObservableProperty]
        public bool isSubmitting = false;

        [ObservableProperty]
        public TarefaAlterarDTO tarefaCadastrarDTO = new TarefaAlterarDTO();

        public CadastrarTarefaViewModel(
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

        public async Task VertificarSecaoValidaAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
            {
                await notificacaoService.MostrarErro("Sessão expirada. Por favor, faça login novamente.");

                navigation.NavigateTo("/login");
                //navigation.NavigateTo("/", forceLoad: true);

            }
        }

        public async Task CadastrarTarefaAsync()
        {
            try
            {
                var (sucesso, errorMessage) = await iTarefaService.CadastrarTarefaAsync(TarefaCadastrarDTO);

                if (sucesso)
                {
                    await notificacaoService.MostrarSucesso("Tarefa cadastrada com sucesso!");

                    LimparCampos();
                }
                else
                {
                    await notificacaoService.MostrarErro(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar tarefas: {ex.Message}");

                await notificacaoService.MostrarErro($"Ocorreu um erro interno. Nossa equipe já foi notificada.");
            }


        }

        private void LimparCampos()
        {
            TarefaCadastrarDTO.Titulo = "";
            TarefaCadastrarDTO.Prioridade = "";
            TarefaCadastrarDTO.Prazo = 0;
            TarefaCadastrarDTO.Descricao = "";
            TarefaCadastrarDTO.Status = "";
        }

        public async Task CancelarAlteracaoAsync()
        {
            bool confirmado = await notificacaoService.MostrarConfirmacao("Atenção!", "Tem certeza que deseja cancelar a alteração da tarefa?");

            if (confirmado)
            {
                navigation.NavigateTo($"/tarefa-rolagem/todas");
            }
        }
    }
}
