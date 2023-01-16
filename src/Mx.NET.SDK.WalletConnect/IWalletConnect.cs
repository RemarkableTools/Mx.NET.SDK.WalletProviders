using Mx.NET.SDK.Domain;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using System;
using System.Threading.Tasks;
using WalletConnectSharp.Core;

namespace Mx.NET.SDK.WalletConnect
{
    public interface IWalletConnect
    {
        string URI { get; }
        string Address { get; }

        public event EventHandler<WalletConnectSession> OnSessionConnected;
        public event EventHandler OnSessionDisconnected;

        bool IsConnected();

        Task Connect();

        Task Disconnect();

        Task<TransactionRequestDto> Sign(TransactionRequest transactionRequest);

        Task<TransactionRequestDto[]> MultiSign(TransactionRequest[] transactionsRequest);
    }
}
