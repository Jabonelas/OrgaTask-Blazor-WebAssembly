using Blazor_WebAssembly.DTOs;
using Newtonsoft.Json;
using System.Text;
using Blazored.LocalStorage;
using Blazor_WebAssembly.Interfaces;
using Blazor_WebAssembly.DTOs.Usuario;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor_WebAssembly.Services.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient http;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authStateProvider;

        public UsuarioService(HttpClient _http, ILocalStorageService _localStorage, AuthenticationStateProvider _authStateProvider)
        {
            http = _http;
            localStorage = _localStorage;
            authStateProvider = _authStateProvider;
        }

        public async Task<(bool success, string errorMessage)> LoginAsync(UsuarioLoginDTO _dadosLogin)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosLogin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

#if DEBUG

                var request = new HttpRequestMessage(HttpMethod.Post, $"usuarios/login")
                {
                    Content = content
                };
#else

    var request = new HttpRequestMessage(HttpMethod.Post, $"https://blazor-api.onrender.com/api/usuarios/login")
                {
                    Content = content
                };

#endif

                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<UserToken>(responseContent);

                    // Armazenando o token
                    await localStorage.SetItemAsync("authToken", result.Token);

                    // Armazenando o usuario
                    await localStorage.SetItemAsync("usuario", _dadosLogin.login);

                    (authStateProvider as CustomAuthenticationStateProvider)?.NotifyUserLogout();

                    return (true, null);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                    return (false, errorResponse?.message ?? "Erro desconhecido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em LoginAsync: {ex.Message}");

                throw;
            }
        }

        public async Task<(bool success, string errorMessage)> CadastrarUsuarioAsync(UsuarioCadastrarDTO _dadosUsuario)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_dadosUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

#if DEBUG

                var request = new HttpRequestMessage(HttpMethod.Post, "usuarios")
                {
                    Content = content
                };
#else

    var request = new HttpRequestMessage(HttpMethod.Post, "https://blazor-api.onrender.com/api/usuarios")
    {
        Content = content
    };

#endif

                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<UserToken>(responseContent);

                    //    // Armazenando o token
                    await localStorage.SetItemAsync("authToken", result.Token);
                    return (true, null);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
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