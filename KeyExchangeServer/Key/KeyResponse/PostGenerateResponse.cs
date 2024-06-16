namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;

public class PostGenerateResponse
{
    public class Root
    {
        public string UniqueId { get; set; }
        public string Key { get; set; }
        public DateTime ExpirationUtc { get; set; }
        public string Algorithm { get; set; }
        public string Certificate { get; set; }
    }
}