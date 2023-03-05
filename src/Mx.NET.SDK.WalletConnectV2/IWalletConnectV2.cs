using Mx.NET.SDK.Domain;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.WalletConnectV2.Models.Events;
using System;
using System.Threading.Tasks;
using WalletConnectSharp.Events.Model;

namespace Mx.NET.SDK.WalletConnectV2
{
    public interface IWalletConnectV2
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

        Task Initialize();

        Task Connect();

        Task Disconnect();

        Task<TransactionRequestDto> Sign(TransactionRequest transactionRequest);

        Task<TransactionRequestDto[]> MultiSign(TransactionRequest[] transactionsRequest);
    }
}
