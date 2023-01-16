using Newtonsoft.Json;
using WalletConnectSharp.Core.Models;

namespace Mx.NET.SDK.WalletConnect.Data
{
    public class WcMultiRequest : JsonRpcRequest
    {
        [JsonProperty("params")]
        private RequestData[] _parameters;

        [JsonIgnore]
        public RequestData[] Parameters => _parameters;

        public WcMultiRequest(RequestData[] transactionDatas) : base()
        {
            Method = "erd_batch_sign";
            _parameters = transactionDatas;
        }
    }
}
