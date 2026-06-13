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
        var query = _context.Livros.AsQueryable();

        if (!string.IsNullOrEmpty(busca))
        {
            query = query.Where(l => l.Titulo.Contains(busca) || l.Autor.Contains(busca));
        }

        var livros = await query.OrderBy(l => l.Titulo).ToListAsync();
        return View(livros);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Sobre()
    {
        return View();
    }

    public async Task<IActionResult> Produtos(string busca)
    {
        var query = _context.Produtos.AsQueryable();

        if (!string.IsNullOrEmpty(busca))
        {
            query = query.Where(p => p.Nome.Contains(busca) || p.Descricao.Contains(busca));
        }

        var listaParaVenda = await query.OrderBy(p => p.Nome).ToListAsync();
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
