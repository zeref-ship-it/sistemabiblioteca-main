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
            query = query.Where(p => p.Nome.Contains(busca));
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
}
