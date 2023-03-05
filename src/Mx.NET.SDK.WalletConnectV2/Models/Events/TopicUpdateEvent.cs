using Newtonsoft.Json;

namespace Mx.NET.SDK.WalletConnectV2.Models.Events
{
    public class TopicUpdateEvent
    {
        [JsonProperty("topic")]
        public string Topic { get; set; }
    }
}
