using Financas.Core;
using System;
using System.Threading.Tasks;

namespace Financas.Domain.Repositories
{
    public interface IPedidoIfoodRepository : IRepository<PedidoIfood>
    {
        Task<bool> PedidoExiste(Guid idNoIfood);
        Task<decimal> ObterTotalGastoEmPedidos(string email);
    }
}
