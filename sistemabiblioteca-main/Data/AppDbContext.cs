using Microsoft.EntityFrameworkCore;
using sistemabiblioteca.Models;

namespace sistemabiblioteca.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Livro> Livros => Set<Livro>();
}
