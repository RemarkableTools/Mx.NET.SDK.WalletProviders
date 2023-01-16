using System;
using Mx.NET.SDK.Domain;
using Mx.NET.SDK.Provider.Dtos.API.Transactions;
using Mx.NET.SDK.WebWallet.Enums;

namespace Mx.NET.SDK.WebWallet
{
    public interface IWebWallet
    {
        Uri CreateLoginUrl(string callbackUrl);

        Uri CreateLogoutUrl(string callbackUrl);

        Uri CreateTransactionUrl(TransactionRequest transaction, string callbackUrl);

        Uri CreateTransactionToSignUrl(TransactionRequest transaction, string callbackUrl);

        Uri CreateTransactionsToSignUrl(TransactionRequest[] transactions, string callbackUrl);

        TransactionRequestDto[] GetTransactionsFromUrl(string urlString, out WalletTransactionStatus status);
    }
}
