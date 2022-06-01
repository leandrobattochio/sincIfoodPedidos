using Financas.Domain;
using Financas.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Financas.Data.Repositories
{
    public class AcessoIfoodRepository : BaseRepository<AcessosIfood>, IAcessoIfoodRepository
    {
        private readonly IMemoryCache _memoryCache;

        public AcessoIfoodRepository(FinancasDbContext context, IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache;
        }

        public async Task AtualizarAcesso(AcessosIfood acesso)
        {
            Update(acesso);
            await SaveChanges();

            // Insere no cache por 10 minutos
            _memoryCache.Set(acesso.Email, acesso, TimeSpan.FromMinutes(10));
        }

        public async Task InserirAcesso(AcessosIfood acesso)
        {
            Insert(acesso);
            await SaveChanges();

            // Insere no cache por 10 minutos
            _memoryCache.Set(acesso.Email, acesso, TimeSpan.FromMinutes(10));
        }

        public async Task<AcessosIfood> ObterPorEmail(string email)
        {
            if (_memoryCache.TryGetValue(email, out AcessosIfood acesso))
            {
                return acesso;
            }
            else
            {
                var retorno = await Context.AcessosIfood
                    .Where(c => c.Email == email)
                    .FirstOrDefaultAsync();

                // Cache
                _memoryCache.Set(email, retorno, TimeSpan.FromMinutes(10));

                return retorno;
            }
        }
    }
}
