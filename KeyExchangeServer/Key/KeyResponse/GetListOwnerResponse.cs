namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;

public class GetListOwnerResponse
{
    public class Item
    {
        public string OwnerCode { get; set; }
        public string PublicKey { get; set; }
        public string UniqueId { get; set; }
        public string RelatedClientId { get; set; }
        public List<string> Usage { get; set; }
        public string Type { get; set; }
        public DateTime ExpirationUtc { get; set; }
        public bool HasPrivateKey { get; set; }
    }

    public class Root
    {
        public List<Item> Items { get; set; }
    }
}