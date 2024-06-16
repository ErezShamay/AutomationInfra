namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;

public class GetKeyIdResponse
{
    public class Root
    {
        public string OwnerCode { get; set; }
        public string PublicKey { get; set; }
        public string UniqueId { get; set; }
        public List<string> Usage { get; set; }
        public string Type { get; set; }
        public DateTime ExpirationUtc { get; set; }
        public string RelatedClientId { get; set; }
        public bool HasPrivateKey { get; set; }
    }
}