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
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Produto>().Property(p => p.Preco).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Pedido>().Property(p => p.Total).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<ItemPedido>().Property(p => p.PrecoUnitario).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Pedido>()
            .HasMany(p => p.Itens)
            .WithOne(i => i.Pedido!)
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
