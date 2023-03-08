using Mx.NET.SDK.Configuration;
using Mx.NET.SDK.Core.Domain;
using Mx.NET.SDK.Domain.Data.Account;
using Mx.NET.SDK.Domain.Data.Network;
using Mx.NET.SDK.Provider;
using Mx.NET.SDK.TransactionsManager;
using Mx.NET.SDK.WalletConnectV2;
using Mx.NET.SDK.WalletConnectV2.Models.Events;
using QRCoder;
using System.Diagnostics;
using WalletConnectSharp.Core.Models.Pairing;
using WalletConnectSharp.Events.Model;
using Address = Mx.NET.SDK.Core.Domain.Values.Address;

namespace WinForms
{
    public partial class MainForm : Form
    {
        const string ChainID = "D";
        IWalletConnectV2 WalletConnectV2 { get; set; }

        NetworkConfig NetworkConfig { get; set; } = default!;
        Account Account { get; set; } = default!;
        readonly MultiversxProvider Provider = new(new MultiversxNetworkConfiguration(Network.DevNet));

        public MainForm()
        {
            InitializeComponent();
            this.ActiveControl = qrCodeImg;
            CheckForIllegalCrossThreadCalls = false;

            var metadata = new Metadata()
            {
                Name = "Mx.NET.WinForms",
                Description = "Mx.NET.WinForms login testing",
                Icons = new[] { "https://remarkable.tools/RemarkableTools.ico" },
                Url = "https://remarkable.tools/"
            };
            WalletConnectV2 = new WalletConnectV2(metadata, ChainID);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            LogMessage("Looking for wallet connection...", SystemColors.ControlText);

            var hasConnection = await WalletConnectV2.GetConnection();
            WalletConnectV2.OnSessionUpdateEvent += OnSessionUpdateEvent;
            WalletConnectV2.OnSessionEvent += OnSessionEvent;
            WalletConnectV2.OnSessionDeleteEvent += OnSessionDeleteEvent;
            WalletConnectV2.OnSessionExpireEvent += OnSessionDeleteEvent;
            WalletConnectV2.OnTopicUpdateEvent += OnTopicUpdateEvent;

            if (hasConnection)
            {
                NetworkConfig = await NetworkConfig.GetFromNetwork(Provider);
                Account = Account.From(await Provider.GetAccount(WalletConnectV2.Address));

                qrCodeImg.Visible = false;
                btnConnect.Visible = false;
                btnDisconnect.Visible = true;

                LogMessage("Wallet connected", Color.ForestGreen);
            }
            else
            {
                LogMessage("Connect to xPortal App...", SystemColors.ControlText);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //BtnConnect_Click(sender, e);
        }

        private void LogMessage(string message, Color? color = null)
        {
            tbConnectionStatus.ForeColor = color ?? Color.Black;
            tbConnectionStatus.Text = message;
        }

        private void OnSessionUpdateEvent(object? sender, GenericEvent<SessionUpdateEvent> @event)
        {
            LogMessage("Wallet connected", Color.ForestGreen);
        }

        private void OnSessionEvent(object? sender, GenericEvent<SessionEvent> @event)
        {
            Debug.WriteLine("Session Event");
        }

        private void OnSessionDeleteEvent(object? sender, EventArgs e)
        {
            btnConnect.Visible = true;
            btnDisconnect.Visible = false;

            LogMessage("Wallet disconnected", Color.Firebrick);
        }

        private void OnTopicUpdateEvent(object? sender, GenericEvent<TopicUpdateEvent> @event)
        {
            Debug.WriteLine("Topic Update Event");
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            await WalletConnectV2.Initialize();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(WalletConnectV2.URI, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            qrCodeImg.BackgroundImage = qrCode.GetGraphic(4);
            qrCodeImg.Visible = true;

            LogMessage("Waiting for wallet connection...", Color.Blue);

            try
            {
                await WalletConnectV2.Connect();
                qrCodeImg.Visible = false;
                btnConnect.Visible = false;
                btnDisconnect.Visible = true;

                NetworkConfig = await NetworkConfig.GetFromNetwork(Provider);
                Account = Account.From(await Provider.GetAccount(WalletConnectV2.Address));

                LogMessage("Wallet connected", Color.ForestGreen);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                LogMessage("Wallet connection was not approved", Color.Gold);
            }
        }

        private async void BtnDisconnect_Click(object sender, EventArgs e)
        {
            await WalletConnectV2.Disconnect();
            btnConnect.Visible = true;
            btnDisconnect.Visible = false;

            LogMessage("Wallet disconnected", Color.Firebrick);
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
                var transactionRequestDto = await WalletConnectV2.Sign(transaction);
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
                var transactionsRequestDto = await WalletConnectV2.MultiSign(transactions);
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