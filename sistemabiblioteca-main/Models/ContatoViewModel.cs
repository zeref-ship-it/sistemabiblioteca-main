using System.ComponentModel.DataAnnotations;

namespace sistemabiblioteca.Models;

public class ContatoViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O assunto é obrigatório.")]
    public string Assunto { get; set; } = string.Empty;

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [StringLength(500, ErrorMessage = "A mensagem deve ter no máximo 500 caracteres.")]
    public string Mensagem { get; set; } = string.Empty;
}