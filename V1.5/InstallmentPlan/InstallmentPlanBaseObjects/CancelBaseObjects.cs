namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;

public class CancelBaseObjects
{
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
        public int refundUnderCancelation { get; set; }
        public int cancelationReason { get; set; }
        public bool partialResponseMapping { get; set; }
    }

    public class TouchPoint
    {
        public string code { get; set; }
        public string version { get; set; }
        public string subVersion { get; set; }
        public int versionedTouchpointId { get; set; }
    }
}