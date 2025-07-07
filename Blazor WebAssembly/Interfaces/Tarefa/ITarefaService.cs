using Blazor_WebAssembly.DTOs.Tarefa;

namespace Blazor_WebAssembly.Interfaces.Tarefa
{
    public interface ITarefaService
    {
        Task<(bool Sucesso, string ErrorMessagem, List<TarefaConsultaDTO> Items, int TotalCount)> ObterTarefasPaginadasAsync(int _pageNumber, int _pageSize, string _status);

        Task<(bool Sucesso, string ErrorMessagem)> DeletarTarefaAsync(int _id);

        Task<(bool Sucesso, string ErrorMessagem)> AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<(bool Sucesso, string ErrorMessagem)> CadastrarTarefaAsync(TarefaCadastrarDTO _dadosTarefa);

        Task<(bool Sucesso, string ErrorMessagem, TarefaConsultaDTO TarefaConsultaDTO)> BuscarTarefaAsync(int _id);

        Task<(bool Sucesso, string ErrorMessagem, TarefaQtdStatusDTO TarefaQtdStatusDTO)> BuscarQtdStatusTarefaAsync();

        Task<(bool Sucesso, string ErrorMessagem, List<TarefaPrioridadeAltaDTO> ListaTarefaPrioridadeAltaDTO)> BuscarTarefasPrioridadeAltaAsync();
    }
}