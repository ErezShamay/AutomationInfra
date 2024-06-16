namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.KeyOwner.KeyOwnerBaseObjects;

public class PostOwnerBaseObjects
{
    public class Root
    {
        public string Code { get; set; }
        public string RequiredSubdomain { get; set; }
        public List<string> AllowedClients { get; set; }
        public string DefaultKeyExpiration { get; set; }
    }
}