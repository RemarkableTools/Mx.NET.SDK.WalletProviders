﻿using System;
using System.Threading.Tasks;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models.Engine;
using Mx.NET.SDK.WalletConnect.Models;
using WalletConnectSharp.Common.Model.Errors;
using static Mx.NET.SDK.WalletConnect.Constants.Operations;
using System.IO;
using WalletConnectSharp.Storage;
using Mx.NET.SDK.WalletConnect.Data;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Core;
using WalletConnectSharp.Sign.Models.Engine.Events;

namespace Mx.NET.SDK.WalletConnect
{
    public class WalletConnectGeneric : IWalletConnectGeneric
    {
        public const int WALLETCONNECT_MULTIVERSX_CHAIN_ID = 508;
        public const string WALLETCONNECT_MULTIVERSX_NAMESPACE = "mvx";

        public const string RELAY_URL = "https://bridge.walletconnect.org";
        public const string MAIAR_BRIDGE_URL = "https://maiar.page.link/?apn=com.elrond.maiar.wallet&isi=1519405832&ibi=com.elrond.maiar.wallet&link=https://xportal.com/";

        private readonly SignClientOptions _dappOptions = default!;
        private readonly ConnectOptions _dappConnectOptions = default!;
        private ConnectedData _walletConnect = default!;
        protected SessionStruct _walletConnectSession = default!;
        protected WalletConnectSignClient _client = default!;

        private string _authToken = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string Signature { get; private set; } = string.Empty;
        public string URI { get => $"{_walletConnect.Uri}&token={_authToken}"; }
        public Uri WalletConnectUri { get => new Uri($"{MAIAR_BRIDGE_URL}?wallet-connect={Uri.EscapeDataString(URI)}"); }

        public WalletConnectGeneric(Metadata metadata, string projectID, string chainID, string filePath = null)
        {
            string dappFilePath;
            if (filePath == null)
                dappFilePath = Path.Combine(".wc", "wc_data.json");
            else
                dappFilePath = filePath;

            _dappOptions = new SignClientOptions()
            {
                Name = metadata.Name,
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
                        WALLETCONNECT_MULTIVERSX_NAMESPACE, new ProposedNamespace()
                        {
                            Methods = new[]
                            {
                                SIGN_TRANSACTION,
                                SIGN_TRANSACTIONS,
                                SIGN_MESSAGE,
                                SIGN_LOGIN_TOKEN,
                                SIGN_NATIVE_AUTH_TOKEN,
                                CANCEL_ACTION
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

        public async Task ClientInit()
        {
            if (_client is null)
            {
                _client = await WalletConnectSignClient.Init(_dappOptions);
                SubscribeToEvents();
            }
        }

        public bool TryReconnect()
        {
            if (_client is null)
                throw new Exception("Client is not initialized");

            if (_client.Find(_dappConnectOptions.RequiredNamespaces).Length > 0)
            {
                Reconnect();
                return true;
            }
            return false;
        }

        public async Task Initialize(string authToken = null)
        {
            if (_client is null)
                throw new Exception("Client is not initialized");

            _authToken = authToken;
            _walletConnect = await _client.Connect(_dappConnectOptions);
        }

        public async Task Connect()
        {
            if (_walletConnect is null)
                throw new Exception("WalletConnect is not initialized");

            _walletConnectSession = await _walletConnect.Approval;

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
            _client.AddressProvider.LoadDefaultsAsync();
        }

        public async Task Disconnect()
        {
            await _client.Disconnect(_walletConnectSession.Topic, Error.FromErrorType(ErrorType.USER_DISCONNECTED));
            Address = string.Empty;
            Signature = string.Empty;
        }

        public async Task<string> SignMessage(string message)
        {
            var request = new SignMessageRequest(Address, message);
            var response = await _client.Request<SignMessageRequest, SignMessageResponse>(_walletConnectSession.Topic, request);
            return response.Signature;
        }

        public async Task<ResponseData> Sign(RequestData requestData)
        {
            var request = new SignTransactionRequest() { Transaction = requestData };
            var response = await _client.Request<SignTransactionRequest, SignTransactionResponse>(_walletConnectSession.Topic, request);
            return new ResponseData()
            {
                Signature = response.Signature,
                Version = response.Version,
                Options = response.Options,
                Guardian = response.Guardian,
                GuardianSignature = response.GuardianSignature
            };
        }

        public async Task<ResponseData[]> MultiSign(RequestData[] requestsData)
        {
            var request = new SignTransactionsRequest() { Transactions = requestsData };
            var response = await _client.Request<SignTransactionsRequest, SignTransactionsResponse>(_walletConnectSession.Topic, request);
            return response.Signatures;
        }

        public event EventHandler<SessionEvent> OnSessionDeleteEvent;
        public event EventHandler<SessionStruct> OnSessionExpireEvent;

        private void SubscribeToEvents()
        {
            _client.SessionDeleted += (sender, @event) =>
            {
                Address = string.Empty;
                Signature = string.Empty;
                OnSessionDeleteEvent?.Invoke(sender, @event);
            };
            _client.SessionExpired += (sender, @event) =>
            {
                Address = string.Empty;
                Signature = string.Empty;
                OnSessionExpireEvent?.Invoke(sender, @event);
            };
        }
    }
}
