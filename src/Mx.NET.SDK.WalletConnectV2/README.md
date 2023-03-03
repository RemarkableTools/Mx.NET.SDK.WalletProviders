# Wallet Connect v1 documentation

### About SDK
The library is used to connect to Maiar wallet and sign transactions.

### How to install?
The content is delivered via nuget package:
##### RemarkableTools.Mx.WalletConnect [![Package](https://img.shields.io/nuget/v/RemarkableTools.Mx.WalletConnect)](https://www.nuget.org/packages/RemarkableTools.Mx.WalletConnect/)

---

### Quick start guide
1. Define the ClientMeta (is showing in Maiar when approving the connection)
```csharp
var clientMeta = new ClientMeta()
    {
        Name = "Mx.NET.SDK.WalletConnect",
        Description = "Mx.NET.SDK.WalletConnect login",
        Icons = new[] { "https://remarkable.tools/favicon.ico" },
        URL = "https://remarkable.tools/"
    };
```

2. Define the Wallet Connection and events
```csharp
IWalletConnect IWalletConnect = new WalletConnect(clientMeta);
IWalletConnect.OnSessionConnected += OnSessionConnectEvent;
IWalletConnect.OnSessionDisconnected += OnSessionDisconnectEvent;
```

3. Show a QR code generated with the link from `IWalletConnect.URI`

4. Start the wallet connection
```csharp
await IWalletConnect.Connect();
```

5. The `OnSessionConnectEvent` is called after the connection is approved in Maiar application.

6. Create a transaction then sign it with IWalletConnect
```csharp
var provider = new MultiversxProvider(new MultiversxNetworkConfiguration(Network.DevNet));
var networkConfig = await NetworkConfig.GetFromNetwork(provider);
var account = Account.From(await provider.GetAccount(IWalletAccount.Address));
var receiverAddress = Address.FromBech32("RECEIVER_ADDRESS");
var transactionRequest = EGLDTransactionRequest.EGLDTransfer(
        networkConfig,
        account,
        receiverAddress,
        ESDTAmount.EGLD("1"));

try
{
    var transaction = await IWalletConnect.Sign(transactionsRequest);
    var response = await provider.SendTransaction(transaction);
    Console.WriteLine(response.TxHash);
}
catch(WalletException wex)
{
    Console.WriteLine($"Wallet Exception: {wex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
```

---

## Basic usage example
A Windows application example can be found [here](https://github.com/RemarkableTools/Mx.NET.Examples).