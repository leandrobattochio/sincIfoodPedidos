using Financas.Core;
using Financas.Domain;
using System.Threading.Tasks;

namespace Financas.Ifood.Service
{
    public interface IIFoodService : ITransientDependency
    {
        Task<string> EnviarCodigoDeConfirmacaoParaEmail(string email);
        Task<string> EnviarCodigoRecebidoEmail(string key, string codigo);
        Task<bool> CompletarLogin(string email, string token);
        Task<(bool sucesso, string mensagemErro)> EnfileirarSincronizacaoDePedidos(string email);
        Task SincronizarPedidos(AcessosIfood acesso);
        Task<decimal> ObterTotalGasto(string email);
    }
}
