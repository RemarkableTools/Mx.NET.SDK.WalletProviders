using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace Mx.NET.SDK.WalletConnectV2.Models
{
    [RpcMethod("multiversx_signLoginToken")]
    [RpcRequestOptions(Clock.TEN_SECONDS, true, 1108)]
    [RpcResponseOptions(Clock.TEN_SECONDS, false, 1109)]
    public class LoginRequest
    {
        public string Token { get; set; }
        public string Address { get; set; }

        public LoginRequest(string token, string address)
        {
            Token = token;
            Address = address;
        }
    }

    public class LoginResponse
    {
        public string Signature { get; set; }
    }
}
