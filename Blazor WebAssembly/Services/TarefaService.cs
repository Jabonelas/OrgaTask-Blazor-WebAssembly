using Blazor_WebAssembly.DTOs;
using Blazor_WebAssembly.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net;
using static System.Net.WebRequestMethods;

using Blazored.LocalStorage;

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
            //Pegando o token que foi gerado
            var token = await _localStorage.GetItemAsync<string>("authToken");
            //Pegando o id do usuario logado
            var idUsuario = await _localStorage.GetItemAsync<int>("idUsuario");

            var request = new HttpRequestMessage(HttpMethod.Get, "Tarefa/1/lista");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Sessão expirada.");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TarefaConsultaDTO>>(content);
        }
    }
}