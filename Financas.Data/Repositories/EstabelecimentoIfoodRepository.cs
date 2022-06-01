using Financas.Domain;
using Financas.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Financas.Data.Repositories
{
    public class EstabelecimentoIfoodRepository : BaseRepository<EstabelecimentoIfood>, IEstabelecimentoIfoodRepository
    {
        public EstabelecimentoIfoodRepository(FinancasDbContext context) : base(context)
        {
        }

        public async Task<bool> EstabelecimentoExiste(Guid idNoIfood)
        {
            return await Context.EstabelecimentosIfood
                .Where(c => c.IdIfood == idNoIfood)
                .AnyAsync();
        }

        public async Task<EstabelecimentoIfood> ObterPorIdNoIfood(Guid idIfood)
        {
            return await Context.EstabelecimentosIfood
                .Where(c => c.IdIfood == idIfood)
                .FirstOrDefaultAsync();
        }
    }
}
