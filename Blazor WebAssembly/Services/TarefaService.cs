using Blazor_WebAssembly.DTOs;
using Blazor_WebAssembly.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net;
using static System.Net.WebRequestMethods;

using Blazored.LocalStorage;

using Blazor_WebAssembly.Pages;
using Blazor_WebAssembly.DTOs.Tarefa;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;

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

        public async Task<(List<TarefaConsultaDTO> Items, int TotalCount)> ObterTarefasPaginadasAsync(int pageNumber, int pageSize)
        {
            try
            {
                UserToken dadosToken = new UserToken();
                dadosToken = await PegarDadosToken();

                // Adiciona os parâmetros de paginação na URL
                var request = new HttpRequestMessage(HttpMethod.Get, $"Tarefa/lista-paginada?pageNumber={pageNumber}&pageSize={pageSize}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

                var response = await http.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Erro ao buscar lista de tarefas: {response.StatusCode} - {errorContent}");
                }

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Assumindo que sua API retorna um objeto com Items e TotalCount
                var result = JsonConvert.DeserializeObject<PagedResult<TarefaConsultaDTO>>(content);

                return (result.Items, result.TotalCount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TarefaConsultaDTO>> ObterTarefasAsync()
        {
            UserToken dadosToken = new UserToken();

            dadosToken = await PegarDadosToken();

            var request = new HttpRequestMessage(HttpMethod.Get, $"Tarefa/lista");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Sessão expirada.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao buscar lista de tarefas: {response.StatusCode} - {errorContent}");
            }
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TarefaConsultaDTO>>(content);
        }

        public async Task<bool> CadastrarTarefaAsync(TarefaAlterarDTO _dadosTarefa)
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                // 1. Serializa os dados para JSON
                var json = JsonConvert.SerializeObject(_dadosTarefa);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 2. Cria a requisição com a URL correta (ajuste conforme sua API)
                var request = new HttpRequestMessage(HttpMethod.Post, $"Tarefa/cadastrar")
                {
                    Content = content
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                // 3. Envia a requisição
                var response = await http.SendAsync(request);

                // 4. Trata os possíveis responses
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Erro ao cadastrar tarefa: {response.StatusCode} - {errorContent}");
                }

                // 5. Retorna true se foi bem sucedido
                return true;
            }
            catch (Exception ex)
            {
                // Log do erro (implemente conforme sua necessidade)
                Console.WriteLine($"Erro em CadastrarTarefaAsync: {ex.Message}");
                throw; // Re-lança a exceção para ser tratada pelo chamador
            }
        }

        public async Task<bool> AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa)
        {
            try
            {
                UserToken dadosToken = await PegarDadosToken();

                // 1. Serializa os dados para JSON
                var json = JsonConvert.SerializeObject(_dadosTarefa);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 2. Cria a requisição com a URL correta (ajuste conforme sua API)
                var request = new HttpRequestMessage(HttpMethod.Put, $"Tarefa/alterar")
                {
                    Content = content
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dadosToken.Token);

                // 3. Envia a requisição
                var response = await http.SendAsync(request);

                // 4. Trata os possíveis responses
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Sessão expirada.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Erro ao alterar tarefa: {response.StatusCode} - {errorContent}");
                }

                // 5. Retorna true se foi bem sucedido
                return true;
            }
            catch (Exception ex)
            {
                // Log do erro (implemente conforme sua necessidade)
                Console.WriteLine($"Erro em AlterarTarefaAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<TarefaAlterarDTO> BuscarTarefaAsync(int _id)
        {
            UserToken dadosToken = new UserToken();

            dadosToken = await PegarDadosToken();

            var request = new HttpRequestMessage(HttpMethod.Get, $"Tarefa/{_id}/buscar");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Sessão expirada.");
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TarefaAlterarDTO>(content);
        }

        private async Task<UserToken> PegarDadosToken()
        {
            UserToken dadosToken = new UserToken();

            //Pegando o token que foi gerado
            dadosToken.Token = await localStorage.GetItemAsync<string>("authToken");

            return dadosToken;
        }

        public async Task DeletarTarefaAsync(int _id)
        {
            UserToken dadosToken = new UserToken();

            dadosToken = await PegarDadosToken();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"Tarefa/deletar/{_id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Sessão expirada.");

            response.EnsureSuccessStatusCode();
        }
    }
}