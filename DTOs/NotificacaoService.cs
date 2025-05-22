// Services/NotificacaoService.cs
using Microsoft.JSInterop;

public class NotificacaoService
{
    private readonly IJSRuntime jsRuntime;

    public NotificacaoService(IJSRuntime _jsRuntime)
    {
        jsRuntime = _jsRuntime;
    }

    public async Task MostrarSucesso(string mensagem)
    {
        await jsRuntime.InvokeVoidAsync("Swal.fire", new
        {
            icon = "success",
            title = "Sucesso!",
            text = mensagem,
            timer = 3000, // 3 segundos
            timerProgressBar = true,
            showConfirmButton = false // Oculta o botão "OK"
        });
    }

    public async Task MostrarErro(string mensagem)
    {
        await jsRuntime.InvokeVoidAsync("Swal.fire", new
        {
            icon = "error",
            title = "Atenção!",
            text = mensagem,
            timer = 3000, // 3 segundos
            timerProgressBar = true,
            showConfirmButton = false // Oculta o botão "OK"
        });
    }

    public class SwalResult
    {
        public bool isConfirmed { get; set; }
        public bool isDismissed { get; set; }
        public bool isDenied { get; set; }
    }

    public async Task<bool> MostrarConfirmacao(string titulo, string mensagem)
    {
        var resultado = await jsRuntime.InvokeAsync<SwalResult>("Swal.fire", new
        {
            title = titulo,
            text = mensagem,
            icon = "warning",
            showCancelButton = true,
            confirmButtonText = "Sim, continuar",
            cancelButtonText = "Não, cancelar",
            reverseButtons = true,
            focusConfirm = false,
            customClass = new
            {
                confirmButton = "btn btn-danger",
                cancelButton = "btn btn-secondary"
            }
        });

        return resultado.isConfirmed;
    }
}