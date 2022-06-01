using Newtonsoft.Json;

namespace Financas.Ifood
{
    public class ReAuthenticateResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
