namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;

public class PostRsaSignPayloadBaseObjects
{
    public class Root
    {
        public string Algorithm { get; set; }
        public string Payload { get; set; }
        public string PrivateKey { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
    }
}