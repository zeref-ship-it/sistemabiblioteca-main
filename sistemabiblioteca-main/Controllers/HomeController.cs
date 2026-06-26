using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemabiblioteca.Data;
using sistemabiblioteca.Models;

namespace sistemabiblioteca.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string busca)
    {
        // Destaques na home: alguns produtos do acervo (com imagem)
        var query = _context.Produtos.AsQueryable();

        if (!string.IsNullOrEmpty(busca))
        {
            query = query.Where(p => p.Nome.Contains(busca)
                || (p.Autor != null && p.Autor.Contains(busca))
                || p.Categoria.Contains(busca));
        }

        ViewBag.Busca = busca;
        var destaques = await query.OrderBy(p => p.Id).Take(string.IsNullOrEmpty(busca) ? 6 : 100).ToListAsync();
        return View(destaques);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Sobre()
    {
        return View();
    }

    public async Task<IActionResult> Produtos(string? busca, string? categoria, decimal? precoMin, decimal? precoMax, string? ordenar)
    {
        var query = _context.Produtos.AsQueryable();

        if (!string.IsNullOrEmpty(busca))
        {
            query = query.Where(p => p.Nome.Contains(busca)
                || p.Descricao.Contains(busca)
                || (p.Autor != null && p.Autor.Contains(busca)));
        }

        if (!string.IsNullOrEmpty(categoria))
            query = query.Where(p => p.Categoria == categoria);

        if (precoMin.HasValue)
            query = query.Where(p => p.Preco >= precoMin.Value);

        if (precoMax.HasValue)
            query = query.Where(p => p.Preco <= precoMax.Value);

        query = ordenar switch
        {
            "preco_asc" => query.OrderBy(p => p.Preco),
            "preco_desc" => query.OrderByDescending(p => p.Preco),
            "nome_desc" => query.OrderByDescending(p => p.Nome),
            _ => query.OrderBy(p => p.Nome),
        };

        // Lista de categorias para o filtro (montada a partir do acervo)
        ViewBag.Categorias = await _context.Produtos
            .Select(p => p.Categoria)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        ViewBag.Busca = busca;
        ViewBag.CategoriaAtual = categoria;
        ViewBag.PrecoMin = precoMin;
        ViewBag.PrecoMax = precoMax;
        ViewBag.Ordenar = ordenar;

        var listaParaVenda = await query.ToListAsync();
        return View(listaParaVenda);
    }

    public IActionResult Contato()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Contato(ContatoViewModel model)
    {
        if (ModelState.IsValid)
        {
            TempData["MensagemSucesso"] = "Obrigado! Sua mensagem foi enviada com sucesso e em breve entraremos em contato.";
            return RedirectToAction("Contato");
        }
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
