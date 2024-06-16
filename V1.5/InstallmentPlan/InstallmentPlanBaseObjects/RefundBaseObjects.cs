namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;

public class RefundBaseObjects
{
    public class Amount
    {
        public decimal value { get; set; }
        public string currencyCode { get; set; }
    }

    public class RequestHeader
    {
        public TouchPoint touchPoint { get; set; }
        public string sessionId { get; set; }
        public string apiKey { get; set; }
        public string cultureName { get; set; }
        public int authenticationType { get; set; }
    }

    public class Root
    {
        public RequestHeader requestHeader { get; set; }
        public string installmentPlanNumber { get; set; }
        public Amount amount { get; set; }
        public int refundStrategy { get; set; }
    }

    public class TouchPoint
    {
        public string code { get; set; }
        public string version { get; set; }
        public string subVersion { get; set; }
        public int versionedTouchpointId { get; set; }
    }
}