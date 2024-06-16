namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.KeyOwner.KeyOwnerResponse;

public class GetOwnerCodeResponse
{
    public class Root
    {
        public string Code { get; set; }
        public string RequiredSubdomain { get; set; }
        public List<string> AllowedClients { get; set; }
        public List<string> Keys { get; set; }
        public string DefaultKeyExpiration { get; set; }
    }
}