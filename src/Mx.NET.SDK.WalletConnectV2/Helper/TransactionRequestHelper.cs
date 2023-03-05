using System.Linq;
using Mx.NET.SDK.Domain;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.WalletConnectV2.Data;
using Mx.NET.SDK.WalletConnectV2.Models;

namespace Mx.NET.SDK.WalletConnectV2.Helper
{
    public static class TransactionRequestHelper
    {
        public static TransactionRequestDto ToDto(this TransactionRequest transaction)
        {
            return new TransactionRequestDto()
            {
                ChainID = transaction.ChainId,
                Data = transaction.Data,
                GasLimit = transaction.GasLimit.Value,
                GasPrice = transaction.GasPrice,
                Nonce = transaction.Nonce,
                Receiver = transaction.Receiver.Bech32,
                Sender = transaction.Sender.Bech32,
                Signature = null,
                Value = transaction.Value.ToString(),
                Version = transaction.Version
            };
        }

        public static SignTransactionRequest GetSignTransactionRequest(this TransactionRequest transaction)
        {
            return new SignTransactionRequest()
            {
                transaction = new RequestData()
                {
                    chainID = transaction.ChainId,
                    data = transaction.Data is null ? "" : transaction.Data,
                    gasLimit = transaction.GasLimit.Value,
                    gasPrice = transaction.GasPrice,
                    nonce = transaction.Nonce,
                    sender = transaction.Sender.Bech32,
                    receiver = transaction.Receiver.Bech32,
                    value = transaction.Value.ToString(),
                    version = transaction.Version
                }
            };
        }

        public static SignTransactionsRequest GetSignTransactionsRequest(this TransactionRequest[] transactions)
        {
            return new SignTransactionsRequest()
            {
                transactions = transactions.Select(transaction => new RequestData()
                {
                    chainID = transaction.ChainId,
                    data = transaction.Data is null ? "" : transaction.Data,
                    gasLimit = transaction.GasLimit.Value,
                    gasPrice = transaction.GasPrice,
                    nonce = transaction.Nonce,
                    sender = transaction.Sender.Bech32,
                    receiver = transaction.Receiver.Bech32,
                    value = transaction.Value.ToString(),
                    version = transaction.Version
                }).ToArray()
            };
        }
    }
}
