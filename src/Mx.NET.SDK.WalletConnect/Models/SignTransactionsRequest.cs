using Mx.NET.SDK.WalletConnect.Data;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace Mx.NET.SDK.WalletConnect.Models
{
    [RpcMethod("multiversx_signTransactions")]
    [RpcRequestOptions(Clock.ONE_DAY, 1108)]
    [RpcResponseOptions(Clock.ONE_DAY, 1109)]
    public class SignTransactionsRequest
    {
        [JsonProperty("transactions")]
        public RequestData[] Transactions;
    }

    public class SignTransactionsResponse
    {
        public ResponseData[] Signatures { get; set; }
    }
}
