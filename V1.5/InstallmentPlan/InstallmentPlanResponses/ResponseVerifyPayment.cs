namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;

public class ResponseVerifyPayment
{
    public class Error
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public string additionalInfo { get; set; }
    }

    public class ResponseHeader
    {
        public bool succeeded { get; set; }
        public List<Error> errors { get; set; }
        public string traceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader responseHeader { get; set; }
        public bool isPaid { get; set; }
        public int originalAmountPaid { get; set; }
    }
}