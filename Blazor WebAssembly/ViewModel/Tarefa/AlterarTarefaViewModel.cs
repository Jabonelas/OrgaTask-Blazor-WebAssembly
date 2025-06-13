using Blazor_WebAssembly.DTOs.Tarefa;
using Blazor_WebAssembly.DTOs.Usuario;
using Blazor_WebAssembly.Interfaces.Tarefa;
using Blazor_WebAssembly.Interfaces.Usuario;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor_WebAssembly.ViewModel.Tarefa
{
    public partial class AlterarTarefaViewModel : ObservableObject
    {
        private readonly ITarefaService iTarefaService;
        private readonly NotificacaoService notificacaoService;
        private readonly NavigationManager navigation;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        [ObservableProperty]
        public bool isSubmitting = false;

        [ObservableProperty]
        public TarefaAlterarDTO tarefaAlterarDTO = new TarefaAlterarDTO();

        public AlterarTarefaViewModel(
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

                (bool success, string errorMessage, TarefaAlterarDTO tarefa) = await iTarefaService.BuscarTarefaAsync(_id);

                TarefaAlterarDTO = tarefa;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        public async Task AlterarTarefaAsync()
        {
            try
            {
                (bool sucesso, string errorMessage) = await iTarefaService.AlterarTarefaAsync(TarefaAlterarDTO);

                if (sucesso)
                {
                    await notificacaoService.MostrarSucesso("Dados da tarefa alterada com sucesso!");

                    LimparCampos();

                    navigation.NavigateTo($"/tarefa-rolagem/todas");
                }
                else
                {
                    await notificacaoService.MostrarErro(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao alterar tarefas: {ex.Message}");

                await notificacaoService.MostrarErro($"Ocorreu um erro interno. Nossa equipe já foi notificada.");
            }
        }


        private void LimparCampos()
        {
            TarefaAlterarDTO.Titulo = "";
            TarefaAlterarDTO.Prioridade = "";
            TarefaAlterarDTO.Prazo = 0;
            TarefaAlterarDTO.Descricao = "";
            TarefaAlterarDTO.Status = "";
        }

        public async Task CancelarAlteracaoAsync()
        {
            bool confirmado = await notificacaoService.MostrarConfirmacao("confirm", "Tem certeza que deseja cancelar a alteração da tarefa?");

            if (confirmado)
            {
                navigation.NavigateTo($"/tarefa-rolagem/todas");
            }
        }

    }
}
