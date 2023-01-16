using System.Linq;
using Mx.NET.SDK.Core.Domain.Helper;
using Mx.NET.SDK.Domain;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.WalletConnect.Data;

namespace Mx.NET.SDK.WalletConnect.Helper
{
    public static class TransactionRequestHelper
    {
        public static RequestData GetRequestData(this TransactionRequest transaction)
        {
            return new RequestData()
            {
                chainId = transaction.ChainId,
                data = transaction.Data is null ? "" : DataCoder.DecodeData(transaction.Data),
                gasLimit = transaction.GasLimit.Value,
                gasPrice = transaction.GasPrice,
                nonce = transaction.Nonce,
                from = transaction.Sender.Bech32,
                to = transaction.Receiver.Bech32,
                amount = transaction.Value.ToString(),
                version = transaction.TransactionVersion
            };
        }

        public static RequestData[] GetRequestsData(this TransactionRequest[] transactions)
        {
            return transactions.Select(transaction => new RequestData()
            {
                chainId = transaction.ChainId,
                data = transaction.Data is null ? "" : DataCoder.DecodeData(transaction.Data),
                gasLimit = transaction.GasLimit.Value,
                gasPrice = transaction.GasPrice,
                nonce = transaction.Nonce,
                from = transaction.Sender.Bech32,
                to = transaction.Receiver.Bech32,
                amount = transaction.Value.ToString(),
                version = transaction.TransactionVersion
            }).ToArray();
        }
    }
}
