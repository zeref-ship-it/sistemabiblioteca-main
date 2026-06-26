using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemabiblioteca.Data;
using sistemabiblioteca.Helpers;
using sistemabiblioteca.Models;

namespace sistemabiblioteca.Controllers;

/// <summary>
/// Controla o carrinho de compras (armazenado em sessão) e a finalização de pedidos.
/// </summary>
public class CarrinhoController : Controller
{
    private const string CarrinhoKey = "Carrinho";
    private readonly AppDbContext _context;

    public CarrinhoController(AppDbContext context)
    {
        _context = context;
    }

    private Carrinho ObterCarrinho()
        => HttpContext.Session.GetObject<Carrinho>(CarrinhoKey) ?? new Carrinho();

    private void SalvarCarrinho(Carrinho carrinho)
        => HttpContext.Session.SetObject(CarrinhoKey, carrinho);

    // Exibe o carrinho
    public IActionResult Index()
    {
        return View(ObterCarrinho());
    }

    // Adiciona um produto ao carrinho
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Adicionar(int produtoId, int quantidade = 1, string? returnUrl = null)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null)
            return NotFound();

        if (quantidade < 1) quantidade = 1;

        var carrinho = ObterCarrinho();
        var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (item != null)
        {
            item.Quantidade += quantidade;
        }
        else
        {
            carrinho.Itens.Add(new CarrinhoItem
            {
                ProdutoId = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                Categoria = produto.Categoria,
                Quantidade = quantidade
            });
        }

        SalvarCarrinho(carrinho);
        TempData["MensagemCarrinho"] = $"\"{produto.Nome}\" foi adicionado à sua mochila!";

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index");
    }

    // Atualiza a quantidade de um item
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Atualizar(int produtoId, int quantidade)
    {
        var carrinho = ObterCarrinho();
        var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (item != null)
        {
            if (quantidade < 1)
                carrinho.Itens.Remove(item);
            else
                item.Quantidade = quantidade;
        }
        SalvarCarrinho(carrinho);
        return RedirectToAction("Index");
    }

    // Remove um item do carrinho
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remover(int produtoId)
    {
        var carrinho = ObterCarrinho();
        carrinho.Itens.RemoveAll(i => i.ProdutoId == produtoId);
        SalvarCarrinho(carrinho);
        TempData["MensagemCarrinho"] = "Item removido da mochila.";
        return RedirectToAction("Index");
    }

    // Esvazia o carrinho
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Limpar()
    {
        SalvarCarrinho(new Carrinho());
        return RedirectToAction("Index");
    }

    // Tela de finalização do pedido (checkout)
    public IActionResult Finalizar()
    {
        var carrinho = ObterCarrinho();
        if (!carrinho.Itens.Any())
            return RedirectToAction("Index");

        return View(new Pedido());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Finalizar(Pedido pedido)
    {
        var carrinho = ObterCarrinho();
        if (!carrinho.Itens.Any())
        {
            TempData["MensagemCarrinho"] = "Sua mochila está vazia.";
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
            return View(pedido);

        pedido.DataPedido = DateTime.Now;
        pedido.Status = "Pendente";
        pedido.Total = carrinho.Total;
        pedido.Itens = carrinho.Itens.Select(i => new ItemPedido
        {
            ProdutoId = i.ProdutoId,
            NomeProduto = i.Nome,
            PrecoUnitario = i.Preco,
            Quantidade = i.Quantidade
        }).ToList();

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // limpa o carrinho após o pedido
        SalvarCarrinho(new Carrinho());

        return RedirectToAction("Confirmacao", new { id = pedido.Id });
    }

    // Confirmação do pedido
    public async Task<IActionResult> Confirmacao(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
            return NotFound();

        return View(pedido);
    }
}
