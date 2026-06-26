namespace sistemabiblioteca.Models;

/// <summary>
/// Item do carrinho de compras (armazenado na sessão do usuário).
/// </summary>
public class CarrinhoItem
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public string? Categoria { get; set; }
    public int Quantidade { get; set; }

    public decimal Subtotal => Preco * Quantidade;
}

/// <summary>
/// Representa o carrinho completo (coleção de itens).
/// </summary>
public class Carrinho
{
    public List<CarrinhoItem> Itens { get; set; } = new();

    public decimal Total => Itens.Sum(i => i.Subtotal);
    public int QuantidadeTotal => Itens.Sum(i => i.Quantidade);
}
