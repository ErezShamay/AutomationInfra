namespace Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.response;

public class CreateMerchantResponse
{
    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public string SplititUniqueMerchantID { get; set; }
        public string MerchantId { get; set; }
        public string BusinessUnitId { get; set; }
        public object NewUserId { get; set; }
        public object NewUserActivationToken { get; set; }
    }
    
    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
        public string TraceId { get; set; }
    }
}