using Microsoft.AspNetCore.Mvc;
using sistemabiblioteca.Helpers;
using sistemabiblioteca.Models;

namespace sistemabiblioteca.ViewComponents;

/// <summary>
/// Exibe a quantidade de itens no carrinho (usado na navbar).
/// </summary>
public class CarrinhoResumoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var carrinho = HttpContext.Session.GetObject<Carrinho>("Carrinho") ?? new Carrinho();
        return View(carrinho);
    }
}
