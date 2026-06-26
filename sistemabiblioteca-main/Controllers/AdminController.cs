using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemabiblioteca.Data;
using sistemabiblioteca.Models;

namespace sistemabiblioteca.Controllers;

// Toda a área administrativa agora exige login (ver AccountController/Program.cs)
[Authorize]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string busca)
    {
        var query = _context.Produtos.AsQueryable();

        if (!string.IsNullOrEmpty(busca))
        {
            query = query.Where(p => p.Nome.Contains(busca)
                || (p.Autor != null && p.Autor.Contains(busca))
                || p.Categoria.Contains(busca));
        }

        var resultado = await query.OrderBy(p => p.Id).ToListAsync();
        return View(resultado);
    }

    public IActionResult Criar() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(Produto p)
    {
        if (!ModelState.IsValid)
            return View(p);

        _context.Produtos.Add(p);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return NotFound();

        return View(produto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Produto p)
    {
        if (!ModelState.IsValid)
            return View(p);

        var produto = await _context.Produtos.FindAsync(p.Id);
        if (produto == null)
            return NotFound();

        produto.Nome = p.Nome;
        produto.Descricao = p.Descricao;
        produto.Preco = p.Preco;
        produto.Categoria = p.Categoria;
        produto.Autor = p.Autor;
        produto.ImagemUrl = p.ImagemUrl;
        produto.Estoque = p.Estoque;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Excluir(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // ══════════════════════════════════════════
    //  GERENCIAMENTO DE PEDIDOS
    // ══════════════════════════════════════════

    public async Task<IActionResult> Pedidos(string? status, string? busca)
    {
        var query = _context.Pedidos.Include(p => p.Itens).AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        if (!string.IsNullOrEmpty(busca))
            query = query.Where(p => p.NomeCliente.Contains(busca) || p.EmailCliente.Contains(busca));

        var pedidos = await query.OrderByDescending(p => p.DataPedido).ToListAsync();

        ViewBag.StatusAtual = status;
        ViewBag.Busca = busca;
        ViewBag.TotalPedidos = pedidos.Count;
        ViewBag.FaturamentoTotal = pedidos.Where(p => p.Status != "Cancelado").Sum(p => p.Total);
        ViewBag.Pendentes = await _context.Pedidos.CountAsync(p => p.Status == "Pendente");

        return View(pedidos);
    }

    public async Task<IActionResult> DetalhesPedido(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
            return NotFound();

        return View(pedido);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AtualizarStatus(int id, string status)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
            return NotFound();

        var permitidos = new[] { "Pendente", "Em Preparo", "Enviado", "Concluído", "Cancelado" };
        if (permitidos.Contains(status))
        {
            pedido.Status = status;
            await _context.SaveChangesAsync();
            TempData["MensagemSucesso"] = $"Status do pedido #{id} atualizado para \"{status}\".";
        }

        return RedirectToAction(nameof(DetalhesPedido), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExcluirPedido(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido != null)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            TempData["MensagemSucesso"] = $"Pedido #{id} excluído.";
        }
        return RedirectToAction(nameof(Pedidos));
    }
}
