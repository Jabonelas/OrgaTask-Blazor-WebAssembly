using Blazor_WebAssembly.DTOs.Tarefa;

namespace Blazor_WebAssembly.Interfaces
{
    public interface ITarefaService
    {
        Task<(bool success, string errorMessage, List<TarefaConsultaDTO>)> ObterTarefasAsync();

        Task<(bool success, string errorMessage, List<TarefaConsultaDTO> Items, int TotalCount)> ObterTarefasPaginadasAsync(int pageNumber, int pageSize);

        Task<(bool success, string errorMessage)> DeletarTarefaAsync(int _id);

        Task<(bool success, string errorMessage)> AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<(bool success, string errorMessage)> CadastrarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<(bool success, string errorMessage, TarefaAlterarDTO)> BuscarTarefaAsync(int _id);
    }
}