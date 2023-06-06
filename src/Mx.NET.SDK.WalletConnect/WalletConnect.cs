using System;
using System.Threading.Tasks;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models.Engine;
using Mx.NET.SDK.WalletConnect.Models;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Core.Models.Pairing;
using WalletConnectSharp.Common.Model.Errors;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.WalletConnect.Helper;
using Mx.NET.SDK.Domain;
using System.Collections.Generic;
using WalletConnectSharp.Events;
using WalletConnectSharp.Events.Model;
using Mx.NET.SDK.WalletConnect.Models.Events;
using static Mx.NET.SDK.WalletConnect.Constants.Events;
using Mx.NET.SDK.WalletConnect.Services;
using System.IO;
using WalletConnectSharp.Storage;

namespace Mx.NET.SDK.WalletConnect
{
    public class WalletConnect : IWalletConnect
    {
        public const int WALLETCONNECT_MULTIVERSX_CHAIN_ID = 508;
        public const string WALLETCONNECT_MULTIVERSX_NAMESPACE = "multiversx";

        public const string RELAY_URL = "https://bridge.walletconnect.org";
        public const string MAIAR_BRIDGE_URL = "https://maiar.page.link/?apn=com.elrond.maiar.wallet&isi=1519405832&ibi=com.elrond.maiar.wallet&link=https://maiar.com/";

        private readonly SignClientOptions _dappOptions = default!;
        private readonly ConnectOptions _dappConnectOptions = default!;
        private ConnectedData _walletConnect = default!;
        private SessionStruct _walletConnectSession = default!;
        private WalletConnectSignClient _client = default!;

        private string _authToken;
        public string Address { get; private set; }
        public string Signature { get; private set; }
        public string URI { get => $"{_walletConnect.Uri}&token={_authToken}"; }
        public Uri WalletConnectUri { get => new Uri($"{MAIAR_BRIDGE_URL}?wallet-connect={Uri.EscapeDataString(URI)}"); }

        public WalletConnect(Metadata metadata, string projectID, string chainID)
        {
            var dappFilePath = Path.Combine(".wc", "wc_data.json");
            _dappOptions = new SignClientOptions()
            {
                ProjectId = projectID,
                Metadata = metadata,
                Storage = new FileSystemStorage(dappFilePath)
            };

            var chain = $"{WALLETCONNECT_MULTIVERSX_NAMESPACE}:{chainID}";
            _dappConnectOptions = new ConnectOptions()
            {
                RequiredNamespaces = new RequiredNamespaces()
                {
                    {
                        WALLETCONNECT_MULTIVERSX_NAMESPACE, new RequiredNamespace()
                        {
                            Methods = new[]
                            {
                                "multiversx_signTransaction",
                                "multiversx_signTransactions",
                                "multiversx_signMessage",
                                "multiversx_signLoginToken",
                                "multiversx_cancelAction"
                            },
                            Chains = new[]
                            {
                                chain
                            },
                            Events = Array.Empty<string>()
                        }
                    }
                }
            };
        }

        public async Task<bool> GetConnection()
        {
            _client = await WalletConnectSignClient.Init(_dappOptions);
            SubscribeToEvents();

            if (_client.Find(_dappConnectOptions.RequiredNamespaces).Length > 0)
            {
                Reconnect();
                return true;
            }
            return false;
        }

        public async Task Initialize()
        {
            _walletConnect = await _client.Connect(_dappConnectOptions);
        }

        public async Task Connect(string authToken = null)
        {
            _walletConnectSession = await _walletConnect.Approval;
            _authToken = authToken;

            var selectedNamespace = _walletConnectSession.Namespaces[WALLETCONNECT_MULTIVERSX_NAMESPACE];
            if (selectedNamespace != null && selectedNamespace.Accounts.Length > 0)
            {
                var currentSession = selectedNamespace.Accounts[0];
                var parameters = currentSession.Split(':');
                Address = parameters[2];
            }

            if (!string.IsNullOrEmpty(_authToken))
            {
                var request = new LoginRequest(_authToken, Address);
                var response = await _client.Request<LoginRequest, LoginResponse>(_walletConnectSession.Topic, request);
                Signature = response.Signature;

                if (!SignatureVerifier.Verify(Address, Signature, _authToken))
                {
                    await Disconnect();
                }

                _authToken = "";
            }
        }

        private void Reconnect()
        {
            _walletConnectSession = _client.Find(_dappConnectOptions.RequiredNamespaces)[0];

            var selectedNamespace = _walletConnectSession.Namespaces[WALLETCONNECT_MULTIVERSX_NAMESPACE];
            if (selectedNamespace != null && selectedNamespace.Accounts.Length > 0)
            {
                var currentSession = selectedNamespace.Accounts[0];
                var parameters = currentSession.Split(':');
                Address = parameters[2];
            }
        }

        public async Task Disconnect()
        {
            await _client.Disconnect(_walletConnectSession.Topic, ErrorResponse.FromErrorType(ErrorType.USER_DISCONNECTED));
            Address = string.Empty;
            Signature = string.Empty;
        }

        public async Task<TransactionRequestDto> Sign(TransactionRequest transactionRequest)
        {
            var request = transactionRequest.GetSignTransactionRequest();
            var response = await _client.Request<SignTransactionRequest, SignTransactionResponse>(_walletConnectSession.Topic, request);

            var transaction = transactionRequest.GetTransactionRequest();
            transaction.Signature = response.Signature;
            return transaction;
        }

        public async Task<TransactionRequestDto[]> MultiSign(TransactionRequest[] transactionsRequest)
        {
            var request = transactionsRequest.GetSignTransactionsRequest();
            var response = await _client.Request<SignTransactionsRequest, SignTransactionsResponse>(_walletConnectSession.Topic, request);

            var transactions = new List<TransactionRequestDto>();
            for (var i = 0; i < response.Signatures.Length; i++)
            {
                var transactionRequestDto = transactionsRequest[i].GetTransactionRequest();
                transactionRequestDto.Signature = response.Signatures[i].Signature;
                transactions.Add(transactionRequestDto);
            }

            return transactions.ToArray();
        }

        public event EventHandler<GenericEvent<SessionUpdateEvent>> OnSessionUpdateEvent;
        public event EventHandler<GenericEvent<SessionEvent>> OnSessionEvent;
        public event EventHandler OnSessionDeleteEvent;
        public event EventHandler OnSessionExpireEvent;
        public event EventHandler<GenericEvent<TopicUpdateEvent>> OnTopicUpdateEvent;

        private void SubscribeToEvents()
        {
            _client.On(SESSION_UPDATE, delegate (object sender, GenericEvent<SessionUpdateEvent> @event)
            {
                OnSessionUpdateEvent?.Invoke(sender, @event);
            });

            _client.On(SESSION_EVENT, delegate (object sender, GenericEvent<SessionEvent> @event)
            {
                OnSessionEvent?.Invoke(sender, @event);
            });

            _client.On(SESSION_DELETE, delegate ()
            {
                OnSessionDeleteEvent?.Invoke(this, EventArgs.Empty);
            });

            _client.On(SESSION_EXPIRE, delegate ()
            {
                OnSessionDeleteEvent?.Invoke(this, EventArgs.Empty);
            });

            _client.Core.Pairing.On(PAIRING_DELETE, delegate (object sender, GenericEvent<TopicUpdateEvent> @event)
            {
                OnTopicUpdateEvent?.Invoke(sender, @event);
            });

            _client.Core.Pairing.On(PAIRING_EXPIRE, delegate (object sender, GenericEvent<TopicUpdateEvent> @event)
            {
                OnTopicUpdateEvent?.Invoke(sender, @event);
            });
        }
    }
}
