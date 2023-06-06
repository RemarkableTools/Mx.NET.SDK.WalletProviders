using Mx.NET.SDK.WalletConnect.Data;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace Mx.NET.SDK.WalletConnect.Models
{
    [RpcMethod("multiversx_signTransaction")]
    [RpcRequestOptions(Clock.ONE_DAY, 1108)]
    [RpcResponseOptions(Clock.ONE_DAY, 1109)]
    public class SignTransactionRequest
    {
        [JsonProperty("transaction")]
        public RequestData Transaction;
    }

    public class SignTransactionResponse
    {
        public string Signature { get; set; }
    }
}
