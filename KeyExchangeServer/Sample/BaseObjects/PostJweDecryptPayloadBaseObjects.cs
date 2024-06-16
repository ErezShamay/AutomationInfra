namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;

public class PostJweDecryptPayloadBaseObjects
{
    public class Root
    {
        public string PrivateKey { get; set; }
        public string JwePayload { get; set; }
    }
}