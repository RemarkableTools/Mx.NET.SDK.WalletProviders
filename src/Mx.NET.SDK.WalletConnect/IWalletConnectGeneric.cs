using Mx.NET.SDK.WalletConnect.Data;
using System;
using System.Threading.Tasks;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine.Events;

namespace Mx.NET.SDK.WalletConnect
{
    public interface IWalletConnectGeneric
    {
        string URI { get; }
        string Address { get; }
        string Signature { get; }
        Uri WalletConnectUri { get; }

        event EventHandler<SessionEvent> OnSessionDeleteEvent;
        event EventHandler<SessionStruct> OnSessionExpireEvent;

        /// <summary>
        /// Initilize the client and subscribe to events, if not already initialized
        /// </summary>
        /// <returns></returns>
        Task ClientInit();

        /// <summary>
        /// Returns true if there is a valid connection saved from last usage, otherwise false.
        /// If the connection is valid, it will automatically reconnect
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        bool TryReconnect();

        /// <summary>
        /// Initialize the WalletConnect client connection
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        Task Initialize(string authToken = null);

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
        /// <returns>Transaction response data</returns>
        Task<ResponseData> Sign(RequestData requestData);

        /// <summary>
        /// Request to xPortal app to sign multiple transactions
        /// </summary>
        /// <param name="requestsData">Transactions request data</param>
        /// <returns>Transactions response data</returns>
        Task<ResponseData[]> MultiSign(RequestData[] requestsData);
    }
}
