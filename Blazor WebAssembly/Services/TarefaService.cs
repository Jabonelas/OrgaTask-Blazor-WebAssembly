using Blazor_WebAssembly.DTOs;
using Blazor_WebAssembly.Interfaces;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Net;
using Blazor_WebAssembly.DTOs.Tarefa;
using System.Net.Http.Headers;
using System.Text;

namespace Blazor_WebAssembly.Services
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

        public async Task<(bool success, string errorMessage, List<TarefaConsultaDTO> Items, int TotalCount)> ObterTarefasPaginadasAsync(int pageNumber, int pageSize)
        {
            UserToken dadosToken = new UserToken();
            dadosToken = await PegarDadosToken();

            var request = new HttpRequestMessage(HttpMethod.Get, $"Tarefa/lista-paginada?pageNumber={pageNumber}&pageSize={pageSize}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return (false, "Sessão expirada. Por favor, faça login novamente.", null, 0);
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);

                return (false, errorResponse?.message ?? "Erro desconhecido", null, 0);
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagedResult<TarefaConsultaDTO>>(content);

            return (true, "", result.Items, result.TotalCount);
        }

        public async Task<(bool success, string errorMessage, List<TarefaConsultaDTO>)> ObterTarefasAsync()
        {
            UserToken dadosToken = await PegarDadosToken();

            if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
            {
                return (false, "Token de autenticação inválido", null);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"Tarefa/lista");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await http.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var listaTarefa = JsonConvert.DeserializeObject<List<TarefaConsultaDTO>>(responseContent);

                return (true, null, listaTarefa);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return (false, "Sessão expirada. Por favor, faça login novamente.", null);
            }

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            return (false, errorResponse?.message ?? "Erro desconhecido", null);
        }

        public async Task<(bool success, string errorMessage)> CadastrarTarefaAsync(TarefaAlterarDTO _dadosTarefa)
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

                var request = new HttpRequestMessage(HttpMethod.Post, $"Tarefa/cadastrar")
                {
                    Content = content
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                var response = await http.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return (false, "Sessão expirada. Por favor, faça login novamente.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em CadastrarTarefaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, string errorMessage)> AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa)
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

                var request = new HttpRequestMessage(HttpMethod.Put, $"Tarefa/alterar")
                {
                    Content = content
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                var response = await http.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return (false, "Sessão expirada. Por favor, faça login novamente.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em AlterarTarefaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, string errorMessage, TarefaAlterarDTO)> BuscarTarefaAsync(int _id)
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
                {
                    return (false, "Token de autenticação inválido", null);
                }

                var request = new HttpRequestMessage(HttpMethod.Get, $"Tarefa/{_id}/buscar");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

                var response = await http.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var tarefa = JsonConvert.DeserializeObject<TarefaAlterarDTO>(responseContent);

                    return (true, null, tarefa);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return (false, "Sessão expirada. Por favor, faça login novamente.", null);
                }

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                return (false, errorResponse?.message ?? "Erro desconhecido", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em BuscarTarefaAsync: {ex}");

                return (false, $"Ocorreu um erro inesperado: {ex.Message}", null);
            }
        }

        private async Task<UserToken> PegarDadosToken()
        {
            UserToken dadosToken = new UserToken();

            //Pegando o token que foi gerado
            dadosToken.Token = await localStorage.GetItemAsync<string>("authToken");

            return dadosToken;
        }

        public async Task<(bool success, string errorMessage)> DeletarTarefaAsync(int _id)
        {
            UserToken dadosToken = await PegarDadosToken();

            if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
            {
                return (false, "Token de autenticação inválido");
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, $"Tarefa/deletar/{_id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await http.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tarefa = JsonConvert.DeserializeObject<TarefaAlterarDTO>(responseContent);

                return (true, null);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return (false, "Sessão expirada. Por favor, faça login novamente.");
            }

            return (true, null);
        }
    }
}