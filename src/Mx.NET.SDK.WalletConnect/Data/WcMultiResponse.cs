using Newtonsoft.Json;
using WalletConnectSharp.Core.Models;

namespace Mx.NET.SDK.WalletConnect.Data
{
    public class WcMultiResponse : JsonRpcResponse
    {
        [JsonProperty]
        private ResponseData[] result;

        [JsonIgnore]
        public ResponseData[] Result => result;
    }
}
