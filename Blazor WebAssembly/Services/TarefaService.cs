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

namespace Blazor_WebAssembly.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public TarefaService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<List<TarefaConsultaDTO>> ObterTarefasAsync()
        {
            UserToken dadosToken = new UserToken();

            dadosToken = await PegarDadosToken();

            var request = new HttpRequestMessage(HttpMethod.Get, $"Tarefa/{dadosToken.idUsuario}/lista");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Sessão expirada.");
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TarefaConsultaDTO>>(content);
        }

        public async Task AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa)
        {
            UserToken dadosToken = new UserToken();

            dadosToken = await PegarDadosToken();

            var request = new HttpRequestMessage(HttpMethod.Put, $"Tarefa/alterar/{dadosToken.idUsuario}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Sessão expirada.");
            }

            response.EnsureSuccessStatusCode();
        }

        private async Task<UserToken> PegarDadosToken()
        {
            UserToken dadosToken = new UserToken();

            //Pegando o token que foi gerado
            dadosToken.Token = await _localStorage.GetItemAsync<string>("authToken");

            //Pegando o id do usuario logado
            dadosToken.idUsuario = await _localStorage.GetItemAsync<int>("idUsuario");

            return dadosToken;
        }

        public async Task DeletarTarefaAsync(int _id)
        {
            UserToken dadosToken = new UserToken();

            dadosToken = await PegarDadosToken();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"Tarefa/deletar/{_id}");
            //var request = new HttpRequestMessage(HttpMethod.Delete, $"Tarefa/deletar?_idTarefa={_id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dadosToken.Token);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Sessão expirada.");

            response.EnsureSuccessStatusCode();
        }
    }
}