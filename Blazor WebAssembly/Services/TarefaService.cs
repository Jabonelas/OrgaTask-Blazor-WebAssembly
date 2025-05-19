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

            //var request = new HttpRequestMessage(HttpMethod.Get, $"tarefas/paginado?pageNumber={pageNumber}&pageSize={pageSize}");

#if DEBUG

                var request = new HttpRequestMessage(HttpMethod.Get, $"tarefas/paginado?pageNumber={pageNumber}&pageSize={pageSize}");

#else

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://blazor-api.onrender.com/api/tarefas/paginado?pageNumber={pageNumber}&pageSize={pageSize}");

#endif

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

#if DEBUG

            var request = new HttpRequestMessage(HttpMethod.Get, "tarefas");

#else

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://blazor-api.onrender.com/api/tarefas");

#endif

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

#if DEBUG

                var request = new HttpRequestMessage(HttpMethod.Post, "tarefas")
                {
                    Content = content
                };
#else

                var request = new HttpRequestMessage(HttpMethod.Post, "https://blazor-api.onrender.com/api/tarefas")
                {
                    Content = content
                };

#endif

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

#if DEBUG

                var request = new HttpRequestMessage(HttpMethod.Put, $"tarefas")
                {
                    Content = content
                };
#else

                var request = new HttpRequestMessage(HttpMethod.Put, $"https://blazor-api.onrender.com/api/tarefas")
                {
                    Content = content
                };

#endif

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

#if DEBUG

                var request = new HttpRequestMessage(HttpMethod.Get, $"tarefas/{_id}");
#else

                var request = new HttpRequestMessage(HttpMethod.Get, $"https://blazor-api.onrender.com/api/tarefas/{_id}");

#endif

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

        public async Task<(bool success, string errorMessage)> DeletarTarefaAsync(int _id)
        {
            UserToken dadosToken = await PegarDadosToken();

            if (dadosToken == null || string.IsNullOrEmpty(dadosToken.Token))
            {
                return (false, "Token de autenticação inválido");
            }

#if DEBUG

            var request = new HttpRequestMessage(HttpMethod.Delete, $"tarefas/{_id}");
#else

            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://blazor-api.onrender.com/api/tarefas/{_id}");

#endif

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