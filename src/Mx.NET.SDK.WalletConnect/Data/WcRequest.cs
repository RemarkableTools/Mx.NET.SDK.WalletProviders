using Newtonsoft.Json;
using WalletConnectSharp.Core.Models;

namespace Mx.NET.SDK.WalletConnect.Data
{
    public class WcRequest : JsonRpcRequest
    {
        [JsonProperty("params")]
        private RequestData _parameters;

        [JsonIgnore]
        public RequestData Parameters => _parameters;

        public WcRequest(RequestData transactionDatas) : base()
        {
            Method = "erd_sign";
            _parameters = transactionDatas;
        }
    }
}
