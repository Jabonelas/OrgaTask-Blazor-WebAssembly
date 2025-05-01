using Blazor_WebAssembly.DTOs;

namespace Blazor_WebAssembly.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> LoginAsync(UsuarioLoginDTO _dadosLogin);
    }
}