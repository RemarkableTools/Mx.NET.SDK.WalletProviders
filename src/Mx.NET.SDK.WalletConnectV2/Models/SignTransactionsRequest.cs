using Mx.NET.SDK.WalletConnectV2.Data;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace Mx.NET.SDK.WalletConnectV2.Models
{
    [RpcMethod("multiversx_signTransactions")]
    [RpcRequestOptions(Clock.TEN_SECONDS, true, 1108)]
    [RpcResponseOptions(Clock.TEN_SECONDS, false, 1109)]
    public class SignTransactionsRequest
    {
        public RequestData[] transactions;
    }

    public class SignTransactionsResponse
    {
        public ResponseData[] Signatures { get; set; }
    }
}
