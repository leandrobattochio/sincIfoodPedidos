using Financas.Core;
using Financas.Ifood.Service;
using System.Threading.Tasks;

namespace Financas.Ifood.TarefasSegundoPlano
{
    public class SincronizarPedidosJob : JobSegundoPlano<SincronizarPedidosJobArgs>
    {
        private readonly IIFoodService _ifoodService;

        public SincronizarPedidosJob(IIFoodService ifoodService)
        {
            _ifoodService = ifoodService;
        }

        public override async Task Execute(SincronizarPedidosJobArgs args)
        {
            await _ifoodService.SincronizarPedidos(args.AcessoIfood);
        }
    }
}
