using Financas.Core;
using System;
using System.Threading.Tasks;

namespace Financas.Domain.Repositories
{
    public interface IEstabelecimentoIfoodRepository : IRepository<EstabelecimentoIfood>
    {
        Task<bool> EstabelecimentoExiste(Guid idNoIfood);
        Task<EstabelecimentoIfood> ObterPorIdNoIfood(Guid idIfood);
    }
}
