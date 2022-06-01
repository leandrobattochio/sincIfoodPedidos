using AutoMapper;
using Financas.Core;
using Financas.Domain;
using Financas.Domain.Repositories;
using Financas.Ifood.Client;
using Financas.Ifood.TarefasSegundoPlano;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Financas.Ifood.Service
{

    public class IFoodService : IIFoodService
    {
        #region Declarar

        private readonly IIFoodClientWrapper _ifoodClient;
        private readonly IAcessoIfoodRepository _iFoodAcessoRepository;
        private readonly IPedidoIfoodRepository _pedidoIfoodRepository;
        private readonly IEstabelecimentoIfoodRepository _estabelecimentoIfoodRepository;

        private readonly AsyncRetryPolicy _unAuthorizedPolicy;
        private readonly IMapper _mapper;

        public IFoodService(IIFoodClientWrapper ifoodClient,
            IAcessoIfoodRepository iFoodRepository,
            IMapper mapper,
            IPedidoIfoodRepository pedidoIfoodRepository,
            IEstabelecimentoIfoodRepository estabelecimentoIfoodRepository)
        {
            _ifoodClient = ifoodClient;
            _iFoodAcessoRepository = iFoodRepository;

            _unAuthorizedPolicy = Policy.Handle<NotAuthorizedIfoodException>()
                .RetryAsync(3,
                async (exception, retryNumber, context) =>
                {
                    var ex = (NotAuthorizedIfoodException)exception;
                    await ResolveUnAuthorized(ex.RefreshToken, ex.Email);
                });

            _mapper = mapper;
            _pedidoIfoodRepository = pedidoIfoodRepository;
            _estabelecimentoIfoodRepository = estabelecimentoIfoodRepository;
        }

        #endregion

        #region Autenticação

        public async Task<string> EnviarCodigoDeConfirmacaoParaEmail(string email)
        {
            var response = await _ifoodClient.GetAuthorizationCodes(email);

            if (!string.IsNullOrEmpty(response.Key))
                return response.Key;
            else
                return "";
        }

        public async Task<string> EnviarCodigoRecebidoEmail(string key, string codigo)
        {
            var response = await _ifoodClient.SendAuthorizationCode(codigo, key);

            if (response != null)
                return response.AccessToken;

            return "";
        }

        public async Task<bool> CompletarLogin(string email, string token)
        {
            var result = await _ifoodClient.AuthenticationPost(email, token);

            if (result != null && result.Authenticated)
            {
                await AtualizarAccessTokenDoUsuarioNoBanco(email, result.AccessToken, result.RefreshToken);
                return true;
            }

            return false;
        }

        #endregion

        #region Pedidos & Sincronização

        // Obtem lista de pedidos do cliente paginada
        public async Task<(bool sucesso, string mensagemErro)> EnfileirarSincronizacaoDePedidos(string email)
        {
            try
            {
                var acesso = await _iFoodAcessoRepository.ObterPorEmail(email);

                if (acesso == null)
                    return (false, "Login inválido");

                JobRunner.Enqueue<SincronizarPedidosJob, SincronizarPedidosJobArgs>(new SincronizarPedidosJobArgs()
                {
                    AcessoIfood = acesso
                });

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task SincronizarPedidos(AcessosIfood acesso)
        {
            if (acesso != null)
            {
                await Autenticar(acesso);

                var listaPedidos = new List<ObterPedidosResult>();

                var pagina = 1;
                List<ObterPedidosResult> pedidos;
                do
                {
                    pedidos = await _unAuthorizedPolicy.ExecuteAsync(async () => await _ifoodClient.ObterMeusPedidos(pagina, 5));

                    if (pedidos.Count > 0)
                    {
                        listaPedidos.AddRange(pedidos);
                        pagina++;
                    }

                    Thread.Sleep(500);
                } while (pedidos.Count > 0);

                await InserirPedidosNoBanco(listaPedidos, acesso);
            }
        }

        private async Task InserirPedidosNoBanco(List<ObterPedidosResult> pedidos, AcessosIfood acesso)
        {
            var listaMapeada = _mapper.Map<List<ObterPedidosResult>, List<PedidoIfood>>(pedidos);

            foreach (var item in listaMapeada)
            {
                var pedidoExiste = await _pedidoIfoodRepository.PedidoExiste(item.IdIfood);

                if (!pedidoExiste)
                {
                    var estabelecimentoExiste = await _estabelecimentoIfoodRepository.EstabelecimentoExiste(item.Estabelecimento.IdIfood);

                    if (!estabelecimentoExiste)
                    {
                        var entity = new EstabelecimentoIfood(item.Estabelecimento.Id, item.Estabelecimento.Nome, item.Estabelecimento.Tipo);
                        await _estabelecimentoIfoodRepository.InsertAndGetId(entity);
                        item.AssociarEstabelecimento(entity);
                    }
                    else
                    {
                        var estabelecimento = await _estabelecimentoIfoodRepository.ObterPorIdNoIfood(item.Estabelecimento.IdIfood);
                        item.AssociarEstabelecimento(estabelecimento);
                    }

                    item.AssociarAcesso(acesso);

                    _pedidoIfoodRepository.Update(item);
                    await _pedidoIfoodRepository.SaveChanges();
                }
            }
        }

        #endregion

        #region Metodos Privados

        // Atualiza os tokens do usuario no banco atraves do email informado
        private async Task AtualizarAccessTokenDoUsuarioNoBanco(string email, string token, string refreshToken)
        {
            var accessTokenBD = await _iFoodAcessoRepository.ObterPorEmail(email);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadToken(token);

            if (accessTokenBD == null)
            {
                var acesso = new AcessosIfood(email, token, jwt.ValidTo, refreshToken);
                await _iFoodAcessoRepository.InserirAcesso(acesso);
            }
            else
            {
                accessTokenBD.AtualizarToken(token, refreshToken, jwt.ValidTo);
                await _iFoodAcessoRepository.AtualizarAcesso(accessTokenBD);
            }
        }


        // Executado quando uma rota que precisa de autorização retorna exception de não autorizado.
        // executado pela Policy
        private async Task ResolveUnAuthorized(string refreshToken, string email)
        {
            var resultado = await _ifoodClient.ReAuthenticate(refreshToken);
            if (resultado != null)
            {
                await AtualizarAccessTokenDoUsuarioNoBanco(email, resultado.AccessToken, resultado.RefreshToken);
                _ifoodClient.SetAuthorization(resultado.AccessToken, resultado.RefreshToken, email);
            }
        }

        // Obter um novo token pelo refreshToken só pela data salva no banco.
        private async Task Autenticar(AcessosIfood acesso)
        {
            if (acesso.IsAccessTokenExpirado())
            {
                var resultado = await _ifoodClient.ReAuthenticate(acesso.RefreshToken);
                if (resultado != null)
                {
                    await AtualizarAccessTokenDoUsuarioNoBanco(acesso.Email, resultado.AccessToken, resultado.RefreshToken);
                    _ifoodClient.SetAuthorization(resultado.AccessToken, resultado.RefreshToken, acesso.Email);
                }
            }
            else
                _ifoodClient.SetAuthorization(acesso.AccessToken, acesso.RefreshToken, acesso.Email);
        }

        public async Task<decimal> ObterTotalGasto(string email)
        {
            return await _pedidoIfoodRepository.ObterTotalGastoEmPedidos(email);
        }

        #endregion
    }
}
