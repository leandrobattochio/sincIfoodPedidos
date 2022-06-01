using Financas.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Financas.Data
{
    public class FinancasDbContext : DbContext
    {
        public FinancasDbContext(DbContextOptions<FinancasDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoIfood>()
                .Property(c => c.TotalPedido)
                .HasPrecision(18, 2);
        }

        public DbSet<AcessosIfood> AcessosIfood { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<PedidoIfood> PedidosIfood { get; set; }
        public DbSet<EstabelecimentoIfood> EstabelecimentosIfood { get; set; }

    }
}
