using Mx.NET.SDK.Configuration;
using Mx.NET.SDK.Core.Domain;
using Mx.NET.SDK.Domain.Data.Account;
using Mx.NET.SDK.Domain.Data.Network;
using Mx.NET.SDK.Provider;
using Mx.NET.SDK.TransactionsManager;
using Mx.NET.SDK.WalletConnectV2;
using Mx.NET.SDK.WalletConnectV2.Helper;
using Mx.NET.SDK.WalletConnectV2.Services;
using QRCoder;
using System.Diagnostics;
using WalletConnectSharp.Core.Models.Pairing;
using Address = Mx.NET.SDK.Core.Domain.Values.Address;

namespace WinForms
{
    public partial class MainForm : Form
    {
        IWalletConnectV2 WalletConnectV2 { get; set; } = default!;
        NetworkConfig? networkConfig { get; set; }
        Account account { get; set; } = default!;
        MultiversxProvider provider = new(new MultiversxNetworkConfiguration(Network.DevNet));
        string ChainID = "D";

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            BtnConnect_Click(sender, e);
        }

        private void LogMessage(string message, Color? color = null)
        {
            tbConnectionStatus.ForeColor = color ?? Color.Black;
            tbConnectionStatus.Text = message;
        }

        //private void OnSessionConnectEvent(object sender, WalletConnectSession session)
        //{
        //    LogMessage("Wallet connected", Color.ForestGreen);
        //}

        //private void OnSessionDisconnectEvent(object sender, EventArgs e)
        //{
        //    LogMessage("Wallet disconnected", Color.Firebrick);
        //}

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            var metadata = new Metadata()
            {
                Name = "Mx.NET.WinForms",
                Description = "Mx.NET.WinForms login testing",
                Icons = new[] { "https://remarkable.tools/RemarkableTools.ico" },
                Url = "https://remarkable.tools/"
            };

            //IWalletConnect.OnSessionConnected += OnSessionConnectEvent;
            //IWalletConnect.OnSessionDisconnected += OnSessionDisconnectEvent;

            var authToken = GenerateAuthToken.Random();
            WalletConnectV2 = new WalletConnectV2(metadata, ChainID, authToken);
            await WalletConnectV2.Initialize();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(WalletConnectV2.URI, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            pictureBox1.BackgroundImage = qrCode.GetGraphic(4);

            LogMessage("Waiting for wallet connection...", Color.Blue);

            try
            {
                await WalletConnectV2.Connect();

                //if (!SignatureVerifier.Verify(WalletConnectV2.Address, WalletConnectV2.Signature, authToken))
                //    throw new Exception("Signature could not be validated");

                networkConfig = await NetworkConfig.GetFromNetwork(provider);
                account = Account.From(await provider.GetAccount(WalletConnectV2.Address));
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

            LogMessage("Wallet disconnected", Color.Firebrick);
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbReceiver.Text) || string.IsNullOrWhiteSpace(tbEGLD.Text)) return;

            await account.Sync(provider);

            var transaction =
                EGLDTransactionRequest.EGLDTransfer(
                networkConfig,
                account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text),
                $"TX");

            try
            {
                var transactionRequestDto = await WalletConnectV2.Sign(transaction);
                var response = await provider.SendTransaction(transactionRequestDto);
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

            await account.Sync(provider);

            var transaction0 = EGLDTransactionRequest.EGLDTransfer(
                networkConfig,
                account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text));
            account.IncrementNonce();

            var transaction1 = EGLDTransactionRequest.EGLDTransfer(
                networkConfig,
                account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text),
                "TX");
            account.IncrementNonce();

            var transaction2 = EGLDTransactionRequest.EGLDTransfer(
                networkConfig,
                account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text),
                "TX 11");
            account.IncrementNonce();

            var transaction3 = EGLDTransactionRequest.EGLDTransfer(
                networkConfig,
                account,
                Address.FromBech32(tbReceiver.Text),
                ESDTAmount.EGLD(tbEGLD.Text),
                "TX AB");
            account.IncrementNonce();

            var transactions = new[] { transaction0, transaction1, transaction2, transaction3 };
            try
            {
                var transactionsRequestDto = await WalletConnectV2.MultiSign(transactions);
                var response = await provider.SendTransactions(transactionsRequestDto);
                MessageBox.Show($"Transactions sent to network");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }
    }
}