using System.ComponentModel.DataAnnotations;
namespace sistemabiblioteca.Models;

public class Livro
{
  [Key]
  public int Id { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 1, ErrorMessage = "O título deve conter entre 1 e 100 caracteres.")]
  [Display(Name = "Título")]
  public string Titulo { get; set; } = string.Empty;

  [Required]
  [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome do autor deve conter entre 1 e 100 caracteres.")]
  [Display(Name = "Autor")]
  public string Autor { get; set; } = string.Empty;
}