namespace Splitit.Automation.NG.Backend.Services.AdminPortal.BaseObjects;

public class PostMerchantIdKeysBaseObjects
{
    public class Root
    {
        public string PublicKey { get; set; }
        public List<string> Usage { get; set; }
        public string Type { get; set; }
        public DateTime ExpirationUtc { get; set; }
        public string RelatedClientId { get; set; }
    }
}