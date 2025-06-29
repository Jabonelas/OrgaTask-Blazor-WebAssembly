using Blazor_WebAssembly.DTOs.Usuario;

namespace Blazor_WebAssembly.Interfaces.Usuario
{
    public interface IUsuarioService
    {
        Task<(bool Sucesso, string ErrorMessagem)> LoginAsync(UsuarioLoginDTO _dadosLogin);

        Task<(bool Sucesso, string ErrorMessagem)> CadastrarUsuarioAsync(UsuarioCadastrarDTO _dadosUsuario);
    }
}