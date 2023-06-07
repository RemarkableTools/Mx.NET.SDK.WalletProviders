using Mx.NET.SDK.Core.Domain;
using Mx.NET.SDK.Domain;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.WalletConnect.Data;
using Mx.NET.SDK.WalletConnect.Models.Events;
using System;
using System.Threading.Tasks;
using WalletConnectSharp.Events.Model;

namespace Mx.NET.SDK.WalletConnect
{
    public interface IWalletConnectGeneric
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

        /// <summary>
        /// Returns true if there is a valid connection saved from last usage, otherwise false.
        /// If the connection is valid, it will automatically reconnects to xPortal app
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        Task<bool> GetConnection();

        /// <summary>
        /// Initialize the WalletConnect client connection, preparing the URI for QR code and so on...
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        Task Initialize(string authToken = null);

        /// <summary>
        /// Returns true if wallet is connected
        /// </summary>
        /// <returns></returns>
        bool IsConnected();

        /// <summary>
        /// Awaits xPortal connection approval and is requesting the NativeAuthToken
        /// </summary>
        /// <returns></returns>
        Task Connect();

        /// <summary>
        /// Disconnects from xPortal wallet
        /// </summary>
        /// <returns></returns>
        Task Disconnect();

        /// <summary>
        /// Sign a message
        /// </summary>
        /// <param name="message">Message to be signed</param>
        /// <returns>Message signature</returns>
        Task<string> SignMessage(string message);

        /// <summary>
        /// Request to xPortal app to sign a transaction
        /// </summary>
        /// <param name="requestData">Transaction request data</param>
        /// <returns>Transaction signature</returns>
        Task<string> Sign(RequestData requestData);

        /// <summary>
        /// Request to xPortal app to sign multiple transactions
        /// </summary>
        /// <param name="requestsData">Transactions request data</param>
        /// <returns>Transactions signature</returns>
        Task<string[]> MultiSign(RequestData[] requestsData);
    }
}
