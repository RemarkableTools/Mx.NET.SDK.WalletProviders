using System;
using System.Threading.Tasks;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models.Engine;
using Mx.NET.SDK.WalletConnectV2.Models;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Core.Models.Pairing;
using WalletConnectSharp.Common.Model.Errors;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.WalletConnectV2.Helper;
using Mx.NET.SDK.Domain;
using System.Collections.Generic;
using WalletConnectSharp.Events;
using WalletConnectSharp.Events.Model;
using Mx.NET.SDK.WalletConnectV2.Models.Events;
using static Mx.NET.SDK.WalletConnectV2.Constants.Events;

namespace Mx.NET.SDK.WalletConnectV2
{
    public class WalletConnectV2 : IWalletConnectV2
    {
        public const int WALLETCONNECT_MULTIVERSX_CHAIN_ID = 508;
        public const string WALLETCONNECT_MULTIVERSX_NAMESPACE = "multiversx";

        public const string RELAY_URL = "https://bridge.walletconnect.org";
        public const string MAIAR_BRIDGE_URL = "https://maiar.page.link/?apn=com.elrond.maiar.wallet&isi=1519405832&ibi=com.elrond.maiar.wallet&link=https://maiar.com/";
        public const string PROJECT_ID = "c7d3aa2b21836c991357e8a56c252962";

        private SignClientOptions _dappOptions = default!;
        private ConnectOptions _dappConnectOptions = default!;
        private ConnectedData _walletConnectV2 = default!;
        private SessionStruct _walletConnectV2Session = default!;
        private WalletConnectSignClient _client = default!;
        private EventDelegator _events = default!;

        private string _authToken;
        public string Address { get; private set; }
        public string Signature { get; private set; }
        public string URI { get => $"{_walletConnectV2.Uri}&token={_authToken}"; }
        public Uri WalletConnectUri { get => new($"{MAIAR_BRIDGE_URL}?wallet-connect={Uri.EscapeDataString(URI)}"); }

        public WalletConnectV2(Metadata metadata, string chainID, string authToken)
        {
            _authToken = authToken;
            _dappOptions = new SignClientOptions()
            {
                ProjectId = PROJECT_ID,
                Metadata = metadata
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

        public async Task Initialize()
        {
            _client = await WalletConnectSignClient.Init(_dappOptions);
            _walletConnectV2 = await _client.Connect(_dappConnectOptions);
            SubscribeToEvents();
        }

        public async Task Connect()
        {
            _walletConnectV2Session = await _walletConnectV2.Approval;
            var selectedNamespace = _walletConnectV2Session.Namespaces[WALLETCONNECT_MULTIVERSX_NAMESPACE];
            if (selectedNamespace != null && selectedNamespace.Accounts.Length > 0)
            {
                var currentSession = selectedNamespace.Accounts[0];
                var parameters = currentSession.Split(':');
                Address = parameters[2];
            }

            //try
            //{
            //    var request = new LoginRequest(_authToken, Address);
            //    var response = await _client.Request<LoginRequest, LoginResponse>(_walletConnectV2Session.Topic, request, ChainID);
            //    Signature = response.Signature;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
        }

        public async Task Disconnect()
        {
            await _client.Disconnect(_walletConnectV2Session.Topic, ErrorResponse.FromErrorType(ErrorType.USER_DISCONNECTED));
            Address = string.Empty;
            Signature = string.Empty;
            _authToken = string.Empty;
        }

        public async Task<TransactionRequestDto> Sign(TransactionRequest transactionRequest)
        {
            var request = transactionRequest.GetSignTransactionRequest();

            try
            {
                var response = await _client.Request<SignTransactionRequest, SignTransactionResponse>(_walletConnectV2Session.Topic, request);
                var transaction = transactionRequest.ToDto();
                transaction.Signature = response.Signature;
                return transaction;
            }
            catch (Exception) { throw; }
        }

        public async Task<TransactionRequestDto[]> MultiSign(TransactionRequest[] transactionsRequest)
        {
            var request = transactionsRequest.GetSignTransactionsRequest();

            try
            {
                var response = await _client.Request<SignTransactionsRequest, SignTransactionsResponse>(_walletConnectV2Session.Topic, request);

                var transactions = new List<TransactionRequestDto>();
                for (var i = 0; i < response.Signatures.Length; i++)
                {
                    var transactionRequestDto = transactionsRequest[i].ToDto();
                    transactionRequestDto.Signature = response.Signatures[i].Signature;
                    transactions.Add(transactionRequestDto);
                }

                return transactions.ToArray();
            }
            catch (Exception) { throw; }
        }

        public event EventHandler OnSessionConnectEvent;
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
