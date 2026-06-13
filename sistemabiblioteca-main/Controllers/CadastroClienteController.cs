using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemabiblioteca.Data;
using sistemabiblioteca.Models;

namespace sistemabiblioteca.Controllers
{
    public class CadastroClienteController : Controller
    {
        private readonly AppDbContext _context;

        public CadastroClienteController(AppDbContext context)
        {
            _context = context;
        }

        // Lista de clientes é informação administrativa, exige login
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes.OrderBy(c => c.Id).ToListAsync();
            return View(clientes);
        }

        // Cadastro continua público (ex.: cliente se cadastrando pelo site)
        public IActionResult Cadastrar() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";
            return RedirectToAction(nameof(Cadastrar));
        }
    }
}
