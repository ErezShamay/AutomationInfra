namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.KeyOwner.KeyOwnerResponse;

public class GetListResponse
{
    public class Item
    {
        public string Code { get; set; }
        public string RequiredSubdomain { get; set; }
        public List<string> AllowedClients { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DefaultKeyExpiration { get; set; }
    }

    public class Root
    {
        public List<Item> Items { get; set; }
    }
}