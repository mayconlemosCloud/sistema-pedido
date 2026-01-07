using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data;

/// <summary>
/// Contexto do banco de dados para a aplicação de processamento de pedidos
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet para a tabela de Produtos
    /// </summary>
    public DbSet<Produto> Produtos { get; set; } = null!;

    /// <summary>
    /// DbSet para a tabela de Pedidos
    /// </summary>
    public DbSet<Pedido> Pedidos { get; set; } = null!;

    /// <summary>
    /// DbSet para a tabela de Itens de Pedidos
    /// </summary>
    public DbSet<ItemPedido> ItensPedidos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(255);
            entity.Property(p => p.Descricao).HasMaxLength(1000);
            entity.Property(p => p.Preco).HasPrecision(10, 2);
            entity.Property(p => p.DataCriacao).HasDefaultValueSql("datetime('now')");
        });


        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.ClienteId).IsRequired();
            entity.Property(p => p.ValorTotal).HasPrecision(10, 2).IsRequired();
            entity.Property(p => p.Status).IsRequired().HasMaxLength(50).HasDefaultValue("CRIADO");
            entity.Property(p => p.DataCriacao).HasDefaultValueSql("datetime('now')");


            entity.HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.PedidoId).IsRequired();
            entity.Property(i => i.ProdutoId).IsRequired();
            entity.Property(i => i.Quantidade).IsRequired();
            entity.Property(i => i.PrecoUnitario).HasPrecision(10, 2).IsRequired();
            entity.Property(i => i.PrecoTotal).HasPrecision(10, 2).IsRequired();


            entity.HasIndex(i => i.PedidoId);
            entity.HasIndex(i => i.ProdutoId);
        });
    }
}
