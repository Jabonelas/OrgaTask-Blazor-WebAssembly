using Blazor_WebAssembly.DTOs;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Text;
using Blazored.LocalStorage;
using Blazor_WebAssembly.Interfaces;
using System.Net.Http.Headers;
using System.Net;

namespace Blazor_WebAssembly.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient http;
        private readonly ILocalStorageService localStorage;

        public UsuarioService(HttpClient _http, ILocalStorageService _localStorage)
        {
            http = _http;
            localStorage = _localStorage;
        }

        public async Task<bool> LoginAsync(UsuarioLoginDTO _dadosLogin)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosLogin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"Usuario/login")
                {
                    Content = content
                };

                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<UserToken>(responseContent);

                    //    // Armazenando o token
                    await localStorage.SetItemAsync("authToken", result.Token);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Erro ao cadastrar tarefa: {response.StatusCode} - {errorContent}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em LoginAsync: {ex.Message}");

                throw;
            }
        }
    }
}