using Financas.Core;
using System.Threading.Tasks;

namespace Financas.Domain.Repositories
{
    public interface IAcessoIfoodRepository : IRepository<AcessosIfood>, ITransientDependency
    {
        Task<AcessosIfood> ObterPorEmail(string email);
        Task InserirAcesso(AcessosIfood acesso);
        Task AtualizarAcesso(AcessosIfood acesso);
    }
}
