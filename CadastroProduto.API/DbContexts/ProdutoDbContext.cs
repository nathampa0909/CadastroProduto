using CadastroProduto.API.Models;
using Microsoft.EntityFrameworkCore;
namespace CadastroProduto.API
{
    public class ProdutoDbContext : DbContext
    {
        public ProdutoDbContext(DbContextOptions<ProdutoDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().Property(p => p.Id).HasMaxLength(36).IsRequired();
            modelBuilder.Entity<Produto>().Property(p => p.Codigo).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<Produto>().Property(p => p.Descricao).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Produto>().Property(p => p.CodigoDepartamento).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<Produto>().Property(p => p.Preco).HasPrecision(10, 2).IsRequired();
            modelBuilder.Entity<Produto>().Property(p => p.Status).IsRequired();
        }
    }
}
