using System.ComponentModel.DataAnnotations;

namespace sistemabiblioteca.Models;

/// <summary>
/// Representa um pedido (compra) feito por um cliente no site.
/// </summary>
public class Pedido
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(120)]
    [Display(Name = "Nome do Cliente")]
    public string NomeCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [Display(Name = "E-mail")]
    public string EmailCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Telefone inválido.")]
    [Display(Name = "Telefone")]
    public string TelefoneCliente { get; set; } = string.Empty;

    [Display(Name = "Endereço de Entrega")]
    [StringLength(250)]
    public string? Endereco { get; set; }

    [StringLength(400)]
    [Display(Name = "Observações")]
    public string? Observacoes { get; set; }

    [Display(Name = "Data do Pedido")]
    public DateTime DataPedido { get; set; } = DateTime.Now;

    [Display(Name = "Total")]
    public decimal Total { get; set; }

    /// <summary>Status: Pendente, Em Preparo, Enviado, Concluído, Cancelado.</summary>
    [Display(Name = "Status")]
    public string Status { get; set; } = "Pendente";

    public List<ItemPedido> Itens { get; set; } = new();
}

/// <summary>
/// Item individual dentro de um pedido (uma linha do pedido).
/// </summary>
public class ItemPedido
{
    public int Id { get; set; }

    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public int ProdutoId { get; set; }

    [StringLength(120)]
    public string NomeProduto { get; set; } = string.Empty;

    public decimal PrecoUnitario { get; set; }

    public int Quantidade { get; set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}
