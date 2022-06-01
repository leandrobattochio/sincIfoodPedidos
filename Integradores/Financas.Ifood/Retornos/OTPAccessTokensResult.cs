using Newtonsoft.Json;

namespace Financas.Ifood
{
    public class OTPAccessTokensResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
