using System.ComponentModel.DataAnnotations;

namespace Blazor_WebAssembly.DTOs.Tarefa;

public class TarefaCadastrarDTO
{
    [Required(ErrorMessage = "O título é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O título deve ter no máximo 50 caracteres.")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatório!")]
    [MaxLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "O prioridade é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O prioridade deve ter no máximo 50 caracteres.")]
    public string Prioridade { get; set; }

    [Required(ErrorMessage = "O prazo é obrigatório!")]
    [Range(1, 999)]
    public int Prazo { get; set; }

    [Required(ErrorMessage = "O status é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O status deve ter no máximo 50 caracteres.")]
    public string Status { get; set; }
}

public class TarefaCadastrarDTOAPI
{
    [Required(ErrorMessage = "O título é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O título deve ter no máximo 50 caracteres.")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatório!")]
    [MaxLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "O prazo é obrigatório!")]
    [Range(1, 999)]
    public int Prazo { get; set; }

    public static implicit operator TarefaCadastrarDTOAPI(TarefaCadastrarDTO _tarefa) =>
        new()
        {
            Titulo = _tarefa.Titulo,
            Descricao = _tarefa.Descricao,
            Prazo = _tarefa.Prazo
        };
}