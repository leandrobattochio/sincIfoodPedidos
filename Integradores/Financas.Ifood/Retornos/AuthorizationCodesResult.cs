using Newtonsoft.Json;

namespace Financas.Ifood
{
    public class AuthorizationCodesResult
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        
        [JsonProperty("timeout_in_seconds")]
        public int Timeout { get; set; }
    }
}
