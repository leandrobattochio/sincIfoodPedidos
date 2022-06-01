using Financas.Ifood.Service;
using Financas.Inputs;
using Financas.Outputs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Financas.Controllers
{
    [ApiController]
    [Route("ifood")]
    public class IntegradorIfoodController
    {
        #region Declarar

        private readonly IIFoodService _iFoodService;

        public IntegradorIfoodController(IIFoodService iFoodService)
        {
            _iFoodService = iFoodService;
        }

        #endregion

        #region Autenticação

        [SwaggerOperation(Summary = "Envia para o email informado o código de acesso requerido pelo iFood para completar o processo de login")]
        [HttpPost("enviar-codigo-email")]
        public async Task<OutEnviarCodigoEmail> EnviarCodigoEmail(InEnviarCodigoEmail input)
        {
            var key = await _iFoodService.EnviarCodigoDeConfirmacaoParaEmail(input.Email);

            var result = new OutEnviarCodigoEmail();

            if (!string.IsNullOrEmpty(key))
            {
                result.Key = key;
                result.PreencherSucesso("Foi enviado para seu e-mail um código de 7 dígitos para confirmar seu login. Verifique seu email.");
            }
            else
                result.PreencherErro("Ocorreu uma falha ao tentar enviar o codigo para confirmar o login para seu email.");

            return result;
        }

        [SwaggerOperation(Summary = "Envia o codigo recebido pela primeira rota junto com a key recebida para completar o processo de login")]
        [HttpPost("informar-codigo-recebido-email")]
        public async Task<OutInformarCodigoDeAcesso> InformarCodigoRecebidoEmail(InInformarCodigoDeAcesso input)
        {
            var token = await _iFoodService.EnviarCodigoRecebidoEmail(input.Key, input.Codigo);

            var result = new OutInformarCodigoDeAcesso();

            if (!string.IsNullOrEmpty(token))
            {
                result.Token = token;
                result.PreencherSucesso("Codigo verificado com sucesso, complete o login.");
            }
            else
                result.PreencherErro("Erro ao validar o codigo informado");

            return result;
        }

        [SwaggerOperation(Summary = "Completa o login no iFood e recebe o AccessToken")]
        [HttpPost("completar-login")]
        public async Task<OutBaseDto> CompletarLogin(InCompletarLogin input)
        {
            var sucesso = await _iFoodService.CompletarLogin(input.Email, input.Token);

            var result = new OutBaseDto();

            if (sucesso)
                result.PreencherSucesso("Login efetuado com sucesso!");
            else
                result.PreencherErro("Erro ao efetuar login");

            return result;
        }

        #endregion

        #region Sincronizar Pedidos

        [SwaggerOperation(Summary = "Poe na fila o processo de sincronizar os pedidos")]
        [HttpPost("sincronizar-pedidos")]
        public async Task<OutBaseDto> SincronizarPedidos(InSincronizarPedidos input)
        {
            var (sucesso, mensagemErro) = await _iFoodService.EnfileirarSincronizacaoDePedidos(input.Email);

            var retorno = new OutBaseDto();

            if (sucesso)
                retorno.PreencherSucesso("Seus pedidos serão sincronizados em breve!");
            else
                retorno.PreencherErro(mensagemErro);

            return retorno;
        }

        #endregion

        [SwaggerOperation(Summary = "Obtem total gasto em pedidos")]
        [HttpPost("obter-total-gasto")]
        public async Task<OutTotalGasto> ObterTotalGasto(InTotalGasto input)
        {
            var total = await _iFoodService.ObterTotalGasto(input.Email);

            var retorno = new OutTotalGasto()
            {
                TotalGasto = total
            };
        
            retorno.PreencherSucesso();
            return retorno;
        }
    }
}
