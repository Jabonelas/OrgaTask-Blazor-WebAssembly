using Blazor_WebAssembly.DTOs.Usuario;

namespace Blazor_WebAssembly.Interfaces
{
    public interface IUsuarioService
    {
        Task<(bool success, string errorMessage)> LoginAsync(UsuarioLoginDTO _dadosLogin);

        Task<(bool success, string errorMessage)> CadastrarUsuarioAsync(UsuarioCadastrarDTO _dadosUsuario);
    }
}