using Blazor_WebAssembly.DTOs;
using Blazor_WebAssembly.DTOs.Usuario;
using Blazor_WebAssembly.Helpers;
using Blazor_WebAssembly.Interfaces.Usuario;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Text;

namespace Blazor_WebAssembly.Services.Usuario
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

        public async Task<(bool Sucesso, string ErrorMessagem)> LoginAsync(UsuarioLoginDTO _dadosLogin)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosLogin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var endpoint = ApiRoutes.SetandoEndPoint("api/usuarios/login");

                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Content = content;
                    var response = await http.SendAsync(request);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<UserToken>(responseContent);

                        // Armazenando o token
                        await localStorage.SetItemAsync("authToken", result.Token);

                        // Armazenando o usuario
                        await localStorage.SetItemAsync("usuario", _dadosLogin.Login);

                        return (true, null);
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em LoginAsync: {ex.Message}");

                throw;
            }
        }

        public async Task<(bool Sucesso, string ErrorMessagem)> CadastrarUsuarioAsync(UsuarioCadastrarDTO _dadosUsuario)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var endpoint = ApiRoutes.SetandoEndPoint("api/usuarios");

                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Content = content;

                    var response = await http.SendAsync(request);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return (true, null);
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em CadastrarUsuarioAsync: {ex.Message}");

                return (false, ex.Message);
            }
        }




    }
}