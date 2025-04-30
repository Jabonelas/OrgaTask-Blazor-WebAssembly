using Blazor_WebAssembly.DTOs;
using Blazor_WebAssembly.DTOs.Tarefa;

namespace Blazor_WebAssembly.Interfaces
{
    public interface ITarefaService
    {
        Task<List<TarefaConsultaDTO>> ObterTarefasAsync();

        Task DeletarTarefaAsync(int _id);

        Task<bool> AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<bool> CadastrarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<TarefaAlterarDTO> BuscarTarefaAsync(int _id);
    }
}