# Web Wallet documentation

### About SDK
The library is used to connect to MultiversX Web Wallet and sign transactions.

### How to install?
The content is delivered via nuget package:
##### RemarkableTools.Mx.WebWallet [![Package](https://img.shields.io/nuget/v/RemarkableTools.Mx.WebWallet)](https://www.nuget.org/packages/RemarkableTools.Mx.WebWallet/)

---

### Quick start guide
1. Define the network provider which can be MainNet/DevNet/TestNet
```csharp
var provider = new MultiversxProvider(new MultiversxNetworkConfiguration(Network.DevNet));
```

2. Define the Web Wallet
```csharp
IWebWallet IWebWallet = new WebWallet(provider.NetworkConfiguration);
```

3. Create transaction to sign
```csharp
var networkConfig = await NetworkConfig.GetFromNetwork(provider);
var account = Account.From(await provider.GetAccount("USER_ACCOUNT_ADDRESS"));
var receiverAddress = Address.FromBech32("RECEIVER_ADDRESS");
var transactionRequest = EGLDTransactionRequest.EGLDTransfer(
        networkConfig,
        account,
        receiverAddress,
        ESDTAmount.EGLD("1"));
var transactionToSignUrl = IWebWallet.CreateTransactionToSignUrl(transactionRequest, "https://website.com");
```
4. Navigate to `transactionToSignUrl` and sign the transaction<br />
*callback website address after signing transaction is https://website.com and is followed by some arguments `?TRANSACTION_ARGUMENTS`

5. Get the transaction from URL (*https://website.com?TRANSACTION_ARGUMENTS*)
```csharp
WalletTransactionStatus status;
var transaction = IWebWallet.GetTransactionsFromUrl("https://website.com?TRANSACTION_ARGUMENTS", out status);
switch(status)
{
    case WalletTransactionStatus.TransactionsSigned:
        var response = await provider.SendTransactions(transaction);
        Console.WriteLine(response.TxHash);
        break;
    case WalletTransactionStatus.Cancelled:
        Console.WriteLine("Transaction was canceled");
        break;
    case WalletTransactionStatus.Unknown:
        Console.WriteLine("Some error occured");
        break;
}
```
