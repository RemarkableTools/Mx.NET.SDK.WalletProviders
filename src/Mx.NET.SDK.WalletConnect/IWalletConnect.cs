using Mx.NET.SDK.Domain;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.WalletConnect.Models.Events;
using System;
using System.Threading.Tasks;
using WalletConnectSharp.Events.Model;

namespace Mx.NET.SDK.WalletConnect
{
    public interface IWalletConnect
    {
        string URI { get; }
        string Address { get; }
        string Signature { get; }
        Uri WalletConnectUri { get; }

        event EventHandler<GenericEvent<SessionUpdateEvent>> OnSessionUpdateEvent;
        event EventHandler<GenericEvent<SessionEvent>> OnSessionEvent;
        event EventHandler OnSessionDeleteEvent;
        event EventHandler OnSessionExpireEvent;
        event EventHandler<GenericEvent<TopicUpdateEvent>> OnTopicUpdateEvent;

        Task<bool> GetConnection();

        Task Initialize();

        Task Connect(string authToken = null);

        Task Disconnect();

        Task<TransactionRequestDto> Sign(TransactionRequest transactionRequest);

        Task<TransactionRequestDto[]> MultiSign(TransactionRequest[] transactionsRequest);
    }
}
