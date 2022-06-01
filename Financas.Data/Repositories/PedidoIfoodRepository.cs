using Financas.Domain;
using Financas.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Financas.Data.Repositories
{
    public class PedidoIfoodRepository : BaseRepository<PedidoIfood>, IPedidoIfoodRepository
    {
        public PedidoIfoodRepository(FinancasDbContext context) : base(context)
        {
        }

        public async Task<bool> PedidoExiste(Guid idNoIfood)
        {
            return await Context.PedidosIfood
                    .Where(c => c.IdIfood == idNoIfood)
                    .AnyAsync();
        }

        public async Task<decimal> ObterTotalGastoEmPedidos(string email)
        {
            return await Context.PedidosIfood
                .Include(c => c.AcessoIfood)
                .Where(c => c.AcessoIfood.Email == email)
                .SumAsync(c => c.TotalPedido);
        }
    }
}
