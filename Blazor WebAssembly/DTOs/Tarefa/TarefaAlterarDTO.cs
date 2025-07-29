using System.ComponentModel.DataAnnotations;

namespace Blazor_WebAssembly.DTOs.Tarefa
{
    public class TarefaAlterarDTO
    {
        public int Id { get; set; }

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

        public static implicit operator TarefaAlterarDTO(TarefaConsultaDTO _dadosTarefaCadastro) =>
            new()
            {
                Id = _dadosTarefaCadastro.Id,
                Titulo = _dadosTarefaCadastro.Titulo,
                Descricao = _dadosTarefaCadastro.Descricao,
                Prioridade = _dadosTarefaCadastro.Prioridade,
                Prazo = _dadosTarefaCadastro.Prazo,
                Status = _dadosTarefaCadastro.Status,
            };
    }

    public class TarefaAlterarDTOAPI
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório!")]
        [MaxLength(50, ErrorMessage = "O título deve ter no máximo 50 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatório!")]
        [MaxLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O prazo é obrigatório!")]
        [Range(1, 999)]
        public int Prazo { get; set; }

        public static implicit operator TarefaAlterarDTOAPI(TarefaAlterarDTO _tarefa) =>
            new()
            {
                Id = _tarefa.Id,
                Titulo = _tarefa.Titulo,
                Descricao = _tarefa.Descricao,
                Prazo = _tarefa.Prazo
            };
    }
}