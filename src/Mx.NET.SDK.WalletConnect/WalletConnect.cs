using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WcWalletConnect = WalletConnectSharp.Desktop.WalletConnect;
using Mx.NET.SDK.WalletConnect.Data;
using Mx.NET.SDK.WalletConnect.Helper;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.Domain;

namespace Mx.NET.SDK.WalletConnect
{
    public class WalletConnect : IWalletConnect
    {
        public const int WALLET_CONNECT_MULTIVERSX_CHAIN_ID = 508;

        public const string BRIDGE_URL = "https://bridge.walletconnect.org";
        public const string MAIAR_BRIDGE_URL = "https://maiar.page.link/?apn=com.elrond.maiar.wallet&isi=1519405832&ibi=com.elrond.maiar.wallet&link=https://maiar.com/";

        public string Address { get; set; }
        public string URI { get => _walletConnect.URI ?? ""; }
        public Uri WalletConnectUri { get => new($"{MAIAR_BRIDGE_URL}?wallet-connect={Uri.EscapeDataString(URI)}"); }

        private WcWalletConnect _walletConnect;

        public event EventHandler<WalletConnectSession> OnSessionConnected;
        public event EventHandler OnSessionDisconnected;

        public WalletConnect(ClientMeta clientMeta, string bridgeUrl = BRIDGE_URL)
        {
            _walletConnect = new WcWalletConnect(clientMeta, bridgeUrl, null, null, WALLET_CONNECT_MULTIVERSX_CHAIN_ID);
            _walletConnect.OnSessionConnect += OnSessionConnectEvent;
            _walletConnect.OnSessionDisconnect += OnSessionDisconnectEvent;
        }

        public void OnSessionConnectEvent(object sender, WalletConnectSession session)
        {
            OnSessionConnected?.Invoke(this, session);
        }

        public void OnSessionDisconnectEvent(object sender, EventArgs e)
        {
            OnSessionDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public bool IsConnected()
        {
            return !string.IsNullOrEmpty(Address);
        }

        public async Task Connect()
        {
            await _walletConnect.Connect();
            Address = _walletConnect.Accounts[0];
        }

        public async Task Disconnect()
        {
            await _walletConnect.Disconnect();
            Address = null;
        }

        public async Task<TransactionRequestDto> Sign(TransactionRequest transactionRequest)
        {
            var request = new WcRequest(transactionRequest.GetRequestData());

            try
            {
                var response = await _walletConnect.Send<WcRequest, WcResponse>(request);
                var transaction = transactionRequest.GetTransactionRequest();
                transaction.Signature = response.Result.Signature;
                return transaction;
            }
            catch (WalletException) { throw; }
            catch (Exception) { throw; }
        }

        public async Task<TransactionRequestDto[]> MultiSign(TransactionRequest[] transactionsRequest)
        {
            var requests = new WcMultiRequest(transactionsRequest.GetRequestsData());

            try
            {
                var response = await _walletConnect.Send<WcMultiRequest, WcMultiResponse>(requests);

                var transactions = new List<TransactionRequestDto>();
                for (var i = 0; i < response.Result.Length; i++)
                {
                    var transactionRequestDto = transactionsRequest[i].GetTransactionRequest();
                    transactionRequestDto.Signature = response.Result[i].Signature;
                    transactions.Add(transactionRequestDto);
                }

                return transactions.ToArray();
            }
            catch (WalletException) { throw; }
            catch (Exception) { throw; }
        }
    }
}
