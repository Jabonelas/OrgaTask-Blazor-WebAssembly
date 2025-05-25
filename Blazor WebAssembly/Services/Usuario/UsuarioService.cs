using Blazor_WebAssembly.DTOs;
using Blazor_WebAssembly.DTOs.Usuario;
using Blazor_WebAssembly.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Text;

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

                var endpoint = SetandoEndPoint("usuarios/login");

                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Content = content;
                    var response = await http.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<UserToken>(responseContent);

                        // Armazenando o token
                        await localStorage.SetItemAsync("authToken", result.Token);

                        // Armazenando o usuario
                        await localStorage.SetItemAsync("usuario", _dadosLogin.login);

                        return (true, null);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();

                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                        return (false, errorResponse?.message ?? "Erro desconhecido");
                    }
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

                var endpoint = SetandoEndPoint("usuarios");

                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Content = content;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em CadastrarUsuarioAsync: {ex.Message}");

                return (false, ex.Message);
            }
        }

        #region Metodo Privado

        private string SetandoEndPoint(string _endpont)
        {
#if DEBUG

            return $"{_endpont}";

#else
            return $"https://blazor-api.onrender.com/{_endpont}";

#endif
        }

        #endregion Metodo Privado
    }
}