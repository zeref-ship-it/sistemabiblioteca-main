using System.ComponentModel.DataAnnotations;

namespace sistemabiblioteca.Models;

public class Produto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome deve ter entre 1 e 100 caracteres.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, 1000000, ErrorMessage = "O preço deve ser maior que zero.")]
    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [StringLength(50, ErrorMessage = "A categoria deve ter no máximo 50 caracteres.")]
    [Display(Name = "Categoria")]
    public string Categoria { get; set; } = string.Empty; // Ex: RPG, Fantasia, D&D, Pathfinder...

    [Display(Name = "Autor")]
    [StringLength(100, ErrorMessage = "O autor deve ter no máximo 100 caracteres.")]
    public string? Autor { get; set; }

    [Display(Name = "Imagem (URL)")]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    [Display(Name = "Estoque")]
    [Range(0, 100000, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Estoque { get; set; } = 10;
}
