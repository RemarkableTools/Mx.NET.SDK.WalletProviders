using Mx.NET.SDK.Domain;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using System;
using System.Threading.Tasks;

namespace Mx.NET.SDK.WalletConnectV2
{
    public interface IWalletConnectV2
    {
        string URI { get; }
        string Address { get; }
        string Signature { get; }
        Uri WalletConnectUri { get; }

        Task Initialize();

        Task Connect();

        Task Disconnect();

        Task<TransactionRequestDto> Sign(TransactionRequest transactionRequest);

        Task<TransactionRequestDto[]> MultiSign(TransactionRequest[] transactionsRequest);
    }
}
