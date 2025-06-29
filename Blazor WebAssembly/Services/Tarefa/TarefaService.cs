using Blazor_WebAssembly.DTOs;
using Blazor_WebAssembly.DTOs.Tarefa;
using Blazor_WebAssembly.Helpers;
using Blazor_WebAssembly.Interfaces.Tarefa;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Blazor_WebAssembly.Services.Tarefa
{
    public class TarefaService : ITarefaService
    {
        private readonly HttpClient http;
        private readonly ILocalStorageService localStorage;

        public TarefaService(HttpClient _http, ILocalStorageService _localStorage)
        {
            http = _http;
            localStorage = _localStorage;
        }

        public class PagedResult<T>
        {
            public List<T> Items { get; set; }
            public int TotalCount { get; set; }
        }

        public async Task<(bool Sucesso, string ErrorMessagem, List<TarefaConsultaDTO> Items, int TotalCount)> ObterTarefasPaginadasAsync(int _pageNumber, int _pageSize, string _status)
        {
            try
            {
                UserToken dadosToken = new UserToken();
                dadosToken = await PegarDadosToken();

                var endpoint = ApiRoutes.SetandoEndPoint($"api/tarefas/paginado/{_status}?pageNumber={_pageNumber}&pageSize={_pageSize}");

                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                    var response = await http.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return (false, "Sessão expirada. Por favor, faça login novamente.", null, 0);
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<PagedResult<TarefaConsultaDTO>>(responseContent);

                        return (true, "", result.Items, result.TotalCount);
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido", null, 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em ObterTarefasPaginadasAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}", null, 0);
            }
        }

        public async Task<(bool Sucesso, string ErrorMessagem)> CadastrarTarefaAsync(TarefaAlterarDTO _dadosTarefa)
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
                {
                    return (false, "Token de autenticação inválido");
                }

                var json = JsonConvert.SerializeObject(_dadosTarefa);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var endpoint = ApiRoutes.SetandoEndPoint($"api/tarefas");

                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Content = content;
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                    var response = await http.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return (false, "Sessão expirada. Por favor, faça login novamente.");
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em CadastrarTarefaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool Sucesso, string ErrorMessagem)> AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa)
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
                {
                    return (false, "Token de autenticação inválido");
                }

                var json = JsonConvert.SerializeObject(_dadosTarefa);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var endpoint = ApiRoutes.SetandoEndPoint($"api/tarefas");

                using (var request = new HttpRequestMessage(HttpMethod.Put, endpoint))
                {
                    request.Content = content;
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                    var response = await http.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return (false, "Sessão expirada. Por favor, faça login novamente.");
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em AlterarTarefaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool Sucesso, string ErrorMessagem, TarefaAlterarDTO TarefaAlterarDTO)> BuscarTarefaAsync(int _id)
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
                {
                    return (false, "Token de autenticação inválido", null);
                }

                var endpoint = ApiRoutes.SetandoEndPoint($"api/tarefas/{_id}");

                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                    var response = await http.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return (false, "Sessão expirada. Por favor, faça login novamente.", null);
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var tarefa = JsonConvert.DeserializeObject<TarefaAlterarDTO>(responseContent);

                        return (true, null, tarefa);
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido", null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em BuscarTarefaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}", null);
            }
        }

        public async Task<(bool Sucesso, string ErrorMessagem)> DeletarTarefaAsync(int _id)
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
                {
                    return (false, "Token de autenticação inválido");
                }

                var endpoint = ApiRoutes.SetandoEndPoint($"api/tarefas/{_id}");

                using (var request = new HttpRequestMessage(HttpMethod.Delete, endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                    var response = await http.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return (false, "Sessão expirada. Por favor, faça login novamente.");
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }

                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em DeletarTarefaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool Sucesso, string ErrorMessagem, TarefaQtdStatusDTO TarefaQtdStatusDTO)> BuscarQtdStatusTarefaAsync()
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
                {
                    return (false, "Token de autenticação inválido", null);
                }

                var endpoint = ApiRoutes.SetandoEndPoint("api/tarefas/status_completo");

                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                    var response = await http.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return (false, "Sessão expirada. Por favor, faça login novamente.", null);
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        TarefaQtdStatusDTO qtdStatusTarefas = JsonConvert.DeserializeObject<TarefaQtdStatusDTO>(responseContent);

                        return (true, null, qtdStatusTarefas);
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido", null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em BuscarQtdStatusTarefaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}", null);
            }
        }

        public async Task<(bool Sucesso, string ErrorMessagem, List<TarefaPrioridadeAltaDTO> ListaTarefaPrioridadeAltaDTO)> BuscarTarefasPrioridadeAltaAsync()
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
                {
                    return (false, "Token de autenticação inválido", null);
                }

                var endpoint = ApiRoutes.SetandoEndPoint("api/tarefas/prioridade_alta");

                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                    var response = await http.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return (false, "Sessão expirada. Por favor, faça login novamente.", null);
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var listaTarefasPrioridadeAlta = JsonConvert.DeserializeObject<List<TarefaPrioridadeAltaDTO>>(responseContent);

                        return (true, null, listaTarefasPrioridadeAlta);
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido", null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em BuscarTarefasPrioridadeAltaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}", null);
            }
        }

        #region Métodos privados



        private async Task<UserToken> PegarDadosToken()
        {
            UserToken dadosToken = new UserToken();

            //Pegando o token que foi gerado
            dadosToken.Token = await localStorage.GetItemAsync<string>("authToken");

            return dadosToken;
        }

        #endregion Métodos privados
    }
}