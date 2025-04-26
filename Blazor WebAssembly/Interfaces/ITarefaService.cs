using Blazor_WebAssembly.DTOs;

namespace Blazor_WebAssembly.Interfaces
{
    public interface ITarefaService
    {
        Task<List<TarefaConsultaDTO>> ObterTarefasAsync();
    }
}