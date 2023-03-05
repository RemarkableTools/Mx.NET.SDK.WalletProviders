using Newtonsoft.Json;

namespace Mx.NET.SDK.WalletConnectV2.Models.Events
{
    public class SessionEvent
    {
        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("param")]
        public SessionEventTypes Params { get; set; }
    }

    public class SessionEventTypes
    {
        [JsonProperty("param")]
        public Event Event { get; set; }

        [JsonProperty("chainId")]
        public string ChainId { get; set; }
    }

    public class Event
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }
}
