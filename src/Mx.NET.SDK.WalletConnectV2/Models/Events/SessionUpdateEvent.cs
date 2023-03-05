using Newtonsoft.Json;
using WalletConnectSharp.Sign.Models.Engine.Methods;

namespace Mx.NET.SDK.WalletConnectV2.Models.Events
{
    public class SessionUpdateEvent
    {
        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("params")]
        public SessionUpdate Params { get; set; }
    }
}
