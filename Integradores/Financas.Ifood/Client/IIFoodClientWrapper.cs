using Financas.Core;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Financas.Ifood.Client
{
    public interface IIFoodClientWrapper : ITransientDependency
    {
        // Chamadas HTTP
        Task<AuthorizationCodesResult> GetAuthorizationCodes(string email);
        Task<OTPAccessTokensResult> SendAuthorizationCode(string pinNumber, string key);
        Task<OTP_AuthenticationsResult> AuthenticationPost(string email, string token);
        Task<List<ObterPedidosResult>> ObterMeusPedidos(int page = 0, int size = 10);
        Task<ReAuthenticateResult> ReAuthenticate(string refreshToken);


        // Helpers
        void SetAuthorization(string token, string refreshToken, string email);
    }
}
