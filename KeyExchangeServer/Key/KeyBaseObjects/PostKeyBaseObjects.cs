namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyBaseObjects;

public class PostKeyBaseObjects
{
    public class Root
    {
        public string OwnerCode { get; set; }
        public List<string> Usage { get; set; }
        public string Type { get; set; }
        public string PublicKey { get; set; }
        public DateTime ExpirationUtc { get; set; }
        public string RelatedClientId { get; set; }
    }
}