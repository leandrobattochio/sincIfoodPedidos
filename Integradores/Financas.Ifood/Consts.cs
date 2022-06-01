using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financas.Ifood
{
    public enum IfoodEndpoint
    {
        [Description(Consts.V4 + "/identity-providers")]
        IdentityProviders = 1,

        [Description(Consts.V2 + "/identity-providers/OTP/authorization-codes")]
        OTP_AuthorizationCodes = 2,

        [Description(Consts.V2 + "/identity-providers/OTP/access-tokens")]
        OTP_AccessTokens = 3,

        [Description(Consts.V2 + "/identity-providers/OTP/authentications")]
        OTP_Authentications = 4,

        [Description(Consts.V4 + "/customers/me/orders")]
        Pedidos = 5,

        [Description(Consts.V2 + "/access_tokens")]
        reAuthenticate = 6
    }

    public static class Consts
    {
        public static readonly string MarketPlaceUrl = "https://marketplace.ifood.com.br";


        public const string V2 = "/v2";
        public const string V4 = "/v4";


        public static string ObterUrl(IfoodEndpoint endpoint)
        {
            return $"{MarketPlaceUrl}{endpoint.GetEnumDescription()}";
        }
    }
}
