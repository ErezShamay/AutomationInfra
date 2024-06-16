namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;

public class ResponseV3Update
{
    public class Root
    {
        public string RefOrderNumber { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public string Status { get; set; }
        public string ShippingStatus { get; set; }
        public Error Error { get; set; }
    }
    
    public class Error
    {
        public ExtraData ExtraData { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ExtraData
    {
    }
}