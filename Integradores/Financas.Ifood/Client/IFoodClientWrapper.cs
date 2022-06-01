using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Financas.Domain;
using Financas.Domain.Repositories;
using Financas.Domain.Shared;
using Newtonsoft.Json;
using RestSharp;


namespace Financas.Ifood.Client
{
    public class IFoodClientWrapper : IIFoodClientWrapper
    {
        #region Declarar

        private readonly RestClient _marketPlaceClient;
        
        private string accessToken;
        private string refreshToken;
        private string email;

        private readonly IRequestLogRepository _requestLogRepository;



        public IFoodClientWrapper(IRequestLogRepository requestLogRepository)
        {
            _marketPlaceClient = new RestClient(Consts.MarketPlaceUrl);
            _requestLogRepository = requestLogRepository;
        }

        #endregion

        #region Chamadas HTTP

        #region Autenticação
        // Envia codigo de login para o email e gera uma key.
        public async Task<AuthorizationCodesResult> GetAuthorizationCodes(string email)
        {
            var input = new
            {
                tenant_id = "IFO",
                email = email,
                type = "EMAIL"
            };

            var url = new Uri(Consts.ObterUrl(IfoodEndpoint.OTP_AuthorizationCodes));

            var result = await IfoodHandler(TipoRequisicaoEnum.Post, url, input, async () =>
            {
                var request = new RestRequest(url, Method.Post);
                request.AddJsonBody(input);

                return await _marketPlaceClient.ExecuteAsync(request);
            });

            if (result.Content != null)
                return JsonConvert.DeserializeObject<AuthorizationCodesResult>(result.Content);

            return null;
        }

        public async Task<ReAuthenticateResult> ReAuthenticate(string refreshToken)
        {
            var input = new
            {
                refresh_token = refreshToken
            };

            var url = new Uri(Consts.ObterUrl(IfoodEndpoint.reAuthenticate));

            var result = await IfoodHandler(TipoRequisicaoEnum.Post, url, input, async () =>
            {
                var request = new RestRequest(url, Method.Post);
                request.AddJsonBody(input);

                return await _marketPlaceClient.ExecuteAsync(request);
            });

            var response = JsonConvert.DeserializeObject<ReAuthenticateResult>(result.Content);
            return response;
        }

        public async Task<OTPAccessTokensResult> SendAuthorizationCode(string pinNumber, string key)
        {
            var input = new
            {
                auth_code = pinNumber,
                key = key
            };
            var url = new Uri(Consts.ObterUrl(IfoodEndpoint.OTP_AccessTokens));

            var response = await IfoodHandler(TipoRequisicaoEnum.Post, url, input, async () =>
            {
                var request = new RestRequest(url, Method.Post);
                request.AddJsonBody(input);

                return await _marketPlaceClient.ExecuteAsync(request);
            });

            if (response.Content != null)
            {
                return JsonConvert.DeserializeObject<OTPAccessTokensResult>(response.Content);
            }

            return null;
        }

        public async Task<OTP_AuthenticationsResult> AuthenticationPost(string email, string token)
        {
            var input = new
            {
                device_id = Guid.NewGuid(),
                email = email,
                token = token,
                tenant_id = "IFO"
            };
            var url = new Uri(Consts.ObterUrl(IfoodEndpoint.OTP_Authentications));

            var result = await IfoodHandler(TipoRequisicaoEnum.Post, url, input, async () =>
            {
                var request = new RestRequest(url, Method.Post);
                request.AddJsonBody(input);

                return await _marketPlaceClient.ExecuteAsync(request);
            });

            if (result.Content != null)
            {
                return JsonConvert.DeserializeObject<OTP_AuthenticationsResult>(result.Content);
            }

            return null;
        }

        #endregion
        
        public async Task<List<ObterPedidosResult>> ObterMeusPedidos(int page = 0, int size = 10)
        {
            var url = new Uri(Consts.ObterUrl(IfoodEndpoint.Pedidos));

            var result = await IfoodHandler(TipoRequisicaoEnum.Get, url, "", async () =>
            {
                var request = new RestRequest(url, Method.Get);

                request.AddQueryParameter("page", page);
                request.AddQueryParameter("size", size);
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                var result = await _marketPlaceClient.ExecuteAsync(request);

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                    throw new NotAuthorizedIfoodException(refreshToken, email);

                return result;
            });

            if (result.Content != null)
            {
                try
                {
                    var serialized = JsonConvert.DeserializeObject<List<ObterPedidosResult>>(result.Content);

                    if (serialized != null)
                    {
                        return serialized;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        #endregion

        #region Metodos Internos

        private async Task<RestResponse> IfoodHandler(TipoRequisicaoEnum t, Uri uri, object input, Func<Task<RestResponse>> action)
        {
            var dadosEntrada = JsonConvert.SerializeObject(input);

            var log = new RequestLog(TipoIntegradorEnum.Ifood, dadosEntrada, uri.ToString(), t);

            _requestLogRepository.Insert(log);
            await _requestLogRepository.SaveChanges();

            try
            {
                var response = await action();

                if (response.IsSuccessful)
                    log.AtualizarLogSucesso(response.StatusCode, response.Content);
                else
                    log.AtualizarLogErro(response.StatusCode, response.Content);

                _requestLogRepository.Update(log);
                await _requestLogRepository.SaveChanges();

                return response;

            }
            catch (Exception ex)
            {
                log.AtualizarLogErro(HttpStatusCode.InternalServerError, "", ex.Message);
                throw;
            }
        }

        #endregion

        public void SetAuthorization(string token, string _refreshToken, string _email)
        {
            email = _email;
            accessToken = token;
            refreshToken = _refreshToken;
        }
    }
}
