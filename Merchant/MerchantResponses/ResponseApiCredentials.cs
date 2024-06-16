namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantResponses;

public class ResponseApiCredentials
{
    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public List<ApiUser> ApiUsers { get; set; }
        public bool IsShopifyV1Merchant { get; set; }
    }
    
    public class ApiUser
    {
        public string ClientId { get; set; }
        public bool IsLocked { get; set; }
        public List<RelatedBusinessUnit> RelatedBusinessUnits { get; set; }
        public List<object> Secrets { get; set; }
    }

    public class RelatedBusinessUnit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
        public string TraceId { get; set; }
    }
}