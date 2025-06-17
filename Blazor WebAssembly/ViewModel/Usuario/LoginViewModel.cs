using Blazor_WebAssembly.DTOs.Usuario;
using Blazor_WebAssembly.Interfaces.Usuario;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor_WebAssembly.ViewModel.Usuario
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUsuarioService iUsuarioService;
        private readonly NotificacaoService notificacaoService;
        private readonly IJSRuntime jsRuntime;
        private readonly NavigationManager navigation;

        [ObservableProperty]
        public UsuarioLoginDTO usuarioLogin = new UsuarioLoginDTO();

        [ObservableProperty]
        public bool isSubmitting;

        public LoginViewModel(
            IUsuarioService _iUsuarioService,
            NotificacaoService _notificacaoService,
            IJSRuntime _jsRuntime,
            NavigationManager _navigation)
        {
            iUsuarioService = _iUsuarioService;
            notificacaoService = _notificacaoService;
            jsRuntime = _jsRuntime;
            navigation = _navigation;
        }

        public async Task PegarUsuarioLogadoAsync()
        {
            string usuarioLogado = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "usuario");

            if (!string.IsNullOrEmpty(usuarioLogado))
            {
                usuarioLogin.Login = usuarioLogado.Replace("\"", "");

            }
        }

        public void CriarConta()
        {
            navigation.NavigateTo("/cadastrar-usuario");
        }

        public async Task RealizarLoginAsync()
        {
            isSubmitting = true;

            try
            {
                var (sucesso, mensagemErro) = await iUsuarioService.LoginAsync(usuarioLogin);

                if (sucesso)
                {
                    await notificacaoService.MostrarSucesso("Login realizado com sucesso!");

                    navigation.NavigateTo("/", forceLoad: true);
                }
                else
                {
                    await notificacaoService.MostrarErro(mensagemErro);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao realizar login: {ex.Message}");

                await notificacaoService.MostrarErro("Ocorreu um erro interno. Nossa equipe já foi notificada.");
            }
            finally
            {
                isSubmitting = false;
            }
        }
    }
}
