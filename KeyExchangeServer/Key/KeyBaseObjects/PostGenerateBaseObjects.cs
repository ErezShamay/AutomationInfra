namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyBaseObjects;

public class PostGenerateBaseObjects
{
    public class Root
    {
        public string OwnerCode { get; set; }
        public string Type { get; set; }
        public List<string> Usage { get; set; }
        public int Length { get; set; }
        public DateTime ExpirationUtc { get; set; }
        public string PgpPassPhrase { get; set; }
        public string RelatedClientId { get; set; }
    }
}