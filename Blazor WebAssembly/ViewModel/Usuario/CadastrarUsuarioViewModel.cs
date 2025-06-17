using Blazor_WebAssembly.DTOs.Usuario;
using Blazor_WebAssembly.Interfaces.Usuario;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Blazor_WebAssembly.ViewModel.Usuario
{
    public partial class CadastrarUsuarioViewModel : ObservableObject
    {
        private readonly IUsuarioService iUsuarioService;
        private readonly NotificacaoService notificacaoService;
        private readonly NavigationManager navigation;

        [ObservableProperty]
        public bool isSubmitting = false;

        [ObservableProperty]
        public UsuarioCadastrarDTO usuarioCadastrarDTO = new UsuarioCadastrarDTO();

        [ObservableProperty]
        public UsuarioLoginDTO usuarioLoginDTO = new UsuarioLoginDTO();

        public CadastrarUsuarioViewModel(
            IUsuarioService _iUsuarioService,
            NotificacaoService _notificacaoService,
            NavigationManager _navigation)
        {
            iUsuarioService = _iUsuarioService;
            notificacaoService = _notificacaoService;
            navigation = _navigation;
        }

        public async Task CadastrarUsuarioAsync()
        {
            try
            {
                if (usuarioCadastrarDTO.Senha != usuarioCadastrarDTO.ConfirmarSenha)
                {
                    await notificacaoService.MostrarErro("As senhas não coincidem!");

                    return;
                }

                var (sucesso, errorMessage) = await iUsuarioService.CadastrarUsuarioAsync(usuarioCadastrarDTO);

                if (sucesso)
                {
                    await notificacaoService.MostrarSucesso("Usuário cadastrado com sucesso!");

                    usuarioLoginDTO.Login = usuarioCadastrarDTO.Login;
                    usuarioLoginDTO.Senha = usuarioCadastrarDTO.Senha;

                    await iUsuarioService.LoginAsync(usuarioLoginDTO);

                    LimparCampos();

                    navigation.NavigateTo("/", forceLoad: true);

                }
                else
                {
                    await notificacaoService.MostrarErro(errorMessage);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao cadastrar usuario: {ex.Message}");

                await notificacaoService.MostrarErro($"Ocorreu um erro interno. Nossa equipe já foi notificada.");
            }
        }

        private void LimparCampos()
        {
            usuarioCadastrarDTO.Nome = "";
            usuarioCadastrarDTO.Login = "";
            usuarioCadastrarDTO.Senha = "";
        }

        public async Task CancelarCadastroAsync()
        {
            bool confirmado = await notificacaoService.MostrarConfirmacao("Atenção!", "Tem certeza que deseja cancelar o cadastro do usuário?");

            if (confirmado)
            {
                navigation.NavigateTo($"/");
            }
        }
    }
}
