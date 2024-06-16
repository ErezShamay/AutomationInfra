namespace Splitit.Automation.NG.Backend.Services.Notifications.Merchants.MerchantsResponses;

public class MerchantsResponses
{
    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class Root
    {
        public List<Error> errors { get; set; }
        public int statusCode { get; set; }
        public string traceId { get; set; }
        public bool isSuccess { get; set; }
        public string splititUniqueMerchantID { get; set; }
        public string businessUnitCode { get; set; }
        public int splititMerchantId { get; set; }
        public string tempMerchantId { get; set; }
        public int businessUnitId { get; set; }
        public string response { get; set; }
        public string request { get; set; }
    }
}