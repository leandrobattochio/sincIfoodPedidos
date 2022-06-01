using Newtonsoft.Json;
using System;

namespace Financas.Ifood
{
    public class OTP_AuthenticationsResult
    {
        public bool Authenticated { get; set; }

        [JsonProperty("account_id")]
        public Guid AccountId { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

    }
}
