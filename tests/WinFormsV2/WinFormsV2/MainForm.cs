using Mx.NET.SDK.Configuration;
using Mx.NET.SDK.Core.Domain;
using Mx.NET.SDK.Domain.Data.Account;
using Mx.NET.SDK.Domain.Data.Network;
using Mx.NET.SDK.NativeAuthClient;
using Mx.NET.SDK.NativeAuthClient.Entities;
using Mx.NET.SDK.NativeAuthServer;
using Mx.NET.SDK.NativeAuthServer.Entities;
using Mx.NET.SDK.Provider;
using Mx.NET.SDK.TransactionsManager;
using Mx.NET.SDK.WalletConnect;
using Mx.NET.SDK.WalletConnect.Models.Events;
using QRCoder;
using System.Diagnostics;
using WalletConnectSharp.Core.Models.Pairing;
using WalletConnectSharp.Events.Model;
using Address = Mx.NET.SDK.Core.Domain.Values.Address;

namespace WinForms
{
    public partial class MainForm : Form
    {
        const string CHAIN_ID = "D";
        const string PROJECT_ID = "c7d3aa2b21836c991357e8a56c252962";

        IWalletConnect WalletConnect { get; set; }

        private readonly NativeAuthClient _nativeAuthToken = default!;
        private readonly NativeAuthServer _nativeAuthServer = default!;

        readonly MultiversxProvider Provider = new(new MultiversxNetworkConfiguration(Network.DevNet));
        NetworkConfig NetworkConfig { get; set; } = default!;
        Account Account { get; set; } = default!;

        public MainForm()
        {
            InitializeComponent();
            this.ActiveControl = qrCodeImg;
            CheckForIllegalCrossThreadCalls = false;

            var metadata = new Metadata()
            {
                Name = "Mx.NET.WinForms",
                Description = "Mx.NET.WinForms login testing",
                Icons = new[] { "https://devnet.remarkable.tools/remarkabletools.ico" },
                Url = "https://devnet.remarkable.tools/"
            };
            WalletConnect = new WalletConnect(metadata, PROJECT_ID, CHAIN_ID);
            _nativeAuthToken = new(new NativeAuthClientConfig()
            {
                Origin = metadata.Name,
                ExpirySeconds = 14400,
                BlockHashShard = 2
            });
            var nativeAuthServerConfig = new NativeAuthServerConfig()
            {
                AcceptedOrigins = new[] { metadata.Name }
            };
            _nativeAuthServer = new(nativeAuthServerConfig);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            LogMessage("Looking for wallet connection...", SystemColors.ControlText);

            await WalletConnect.ClientInit();
            WalletConnect.OnSessionUpdateEvent += OnSessionUpdateEvent;
            WalletConnect.OnSessionEvent += OnSessionEvent;
            WalletConnect.OnSessionDeleteEvent += OnSessionDeleteEvent;
            WalletConnect.OnSessionExpireEvent += OnSessionExpireEvent;
            WalletConnect.OnTopicUpdateEvent += OnTopicUpdateEvent;

            try
            {
                var isConnected = WalletConnect.TryReconnect();
                if (isConnected)
                {
                    qrCodeImg.Visible = false;
                    btnConnect.Visible = false;
                    btnDisconnect.Visible = true;

                    LogMessage("Wallet connection restored", Color.ForestGreen);

                    NetworkConfig = await NetworkConfig.GetFromNetwork(Provider);
                    Account = Account.From(await Provider.GetAccount(WalletConnect.Address));
                }
                else
                {
                    btnConnect.Visible = true;

                    LogMessage("Connect with xPortal App", SystemColors.ControlText);
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message, Color.Red);
            }
        }

        private void TbMessageToSign_TextChanged(object sender, EventArgs e)
        {
            lbSignature.Text = string.Empty;
        }

        private void LogMessage(string message, Color? color = null)
        {
            tbConnectionStatus.ForeColor = color ?? Color.Black;
            tbConnectionStatus.Text = message;
        }

        private void OnSessionUpdateEvent(object? sender, GenericEvent<SessionUpdateEvent> @event)
        {
            Debug.WriteLine("Session Update Event");
        }

        private void OnSessionEvent(object? sender, GenericEvent<SessionEvent> @event)
        {
            Debug.WriteLine("Session Event");
        }

        private void OnSessionDeleteEvent(object? sender, EventArgs e)
        {
            NetworkConfig = default!;
            Account = default!;

            btnConnect.Visible = true;
            btnDisconnect.Visible = false;

            LogMessage("Wallet disconnected", Color.Firebrick);
        }

        private void OnSessionExpireEvent(object? sender, EventArgs e)
        {
            btnConnect.Visible = true;
            btnDisconnect.Visible = false;

            NetworkConfig = default!;
            Account = default!;

            LogMessage("Session expired", Color.Firebrick);
        }

        private void OnTopicUpdateEvent(object? sender, GenericEvent<TopicUpdateEvent> @event)
        {
            Debug.WriteLine("Topic Update Event");
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            LogMessage("Generating QR code...", Color.Blue);

            try
            {
                var authToken = await _nativeAuthToken.GenerateToken();
                await WalletConnect.Initialize(authToken);

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(WalletConnect.URI, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                qrCodeImg.BackgroundImage = qrCode.GetGraphic(4);
                qrCodeImg.Visible = true;

                LogMessage("Waiting for wallet connection...", Color.Blue);

                await WalletConnect.Connect();

                try
                {
                    var accessToken = NativeAuthClient.GetAccessToken(WalletConnect.Address, authToken, WalletConnect.Signature);
                    _nativeAuthServer.Validate(accessToken);
                }
                catch
                {
                    await Disconnect();
                    return;
                }

                qrCodeImg.Visible = false;
                btnConnect.Visible = false;
                btnDisconnect.Visible = true;

                NetworkConfig = await NetworkConfig.GetFromNetwork(Provider);
                Account = Account.From(await Provider.GetAccount(WalletConnect.Address));

                LogMessage("Wallet connected", Color.ForestGreen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogMessage("Wallet connection was not approved", Color.Gold);
            }
        }

        private async void BtnDisconnect_Click(object sender, EventArgs e)
        {
            await Disconnect();
        }

        private async Task Disconnect()
        {
            await WalletConnect.Disconnect();

            NetworkConfig = default!;
            Account = default!;

            qrCodeImg.Visible = false;
            btnConnect.Visible = true;
            btnDisconnect.Visible = false;

            LogMessage("Wallet disconnected", Color.Firebrick);
        }

        private async void BtnSignMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMessageToSign.Text)) return;

            try
            {
                var signedMessage = await WalletConnect.SignMessage(tbMessageToSign.Text);
                lbSignature.Text = signedMessage.Signature;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbReceiver.Text) || string.IsNullOrWhiteSpace(tbEGLD.Text)) return;

            await Account.Sync(Provider);

            var transaction =
                EGLDTransactionRequest.EGLDTransfer(
                NetworkConfig,
                Account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text),
                $"TX");

            try
            {
                var transactionRequestDto = await WalletConnect.Sign(transaction);
                var response = await Provider.SendTransaction(transactionRequestDto);
                MessageBox.Show($"Transaction sent to network");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }

        private async void BtnSendMultiple_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbReceiver.Text) || string.IsNullOrWhiteSpace(tbEGLD.Text)) return;

            await Account.Sync(Provider);

            var transaction1 = EGLDTransactionRequest.EGLDTransfer(
                NetworkConfig,
                Account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text));
            Account.IncrementNonce();

            var transaction2 = EGLDTransactionRequest.EGLDTransfer(
                NetworkConfig,
                Account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text),
                "TX");
            Account.IncrementNonce();

            var transaction3 = EGLDTransactionRequest.EGLDTransfer(
                NetworkConfig,
                Account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text),
                "tx AB");
            Account.IncrementNonce();

            var transactions = new[] { transaction1, transaction2, transaction3 };
            try
            {
                var transactionsRequestDto = await WalletConnect.MultiSign(transactions);
                var response = await Provider.SendTransactions(transactionsRequestDto);
                MessageBox.Show($"Transactions sent to network");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }
    }
}