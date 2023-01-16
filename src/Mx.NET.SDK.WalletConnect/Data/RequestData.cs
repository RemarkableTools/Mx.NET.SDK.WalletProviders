namespace Mx.NET.SDK.WalletConnect.Data
{
    public class RequestData
    {
        public string amount { get; set; }
        public string chainId { get; set; }
        public string data { get; set; }
        public string from { get; set; }
        public long gasLimit { get; set; }
        public long gasPrice { get; set; }
        public ulong nonce { get; set; }
        public string to { get; set; }
        public int version { get; set; }
    }
}
