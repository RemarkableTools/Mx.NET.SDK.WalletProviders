using Mx.NET.SDK.Core.Domain.Values;
using Mx.NET.SDK.Core.Domain;
using Mx.NET.SDK.Wallet.Wallet;

namespace Mx.NET.SDK.WalletConnectV2.Services
{
    public static class SignatureVerifier
    {
        public static bool Verify(string address, string signature, string authToken)
        {
            var verifier = WalletVerifier.FromAddress(Address.FromBech32(address));

            var message = new SignableMessage()
            {
                Message = $"{address}{authToken}{{}}",
                Signature = signature
            };

            return verifier.Verify(message);
        }
    }
}
