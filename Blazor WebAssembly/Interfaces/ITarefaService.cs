using Blazor_WebAssembly.DTOs;
using Blazor_WebAssembly.DTOs.Tarefa;

namespace Blazor_WebAssembly.Interfaces
{
    public interface ITarefaService
    {
        Task<List<TarefaConsultaDTO>> ObterTarefasAsync();

        Task DeletarTarefaAsync(int _id);

        Task AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa);
    }
}