namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanResponses;

public class VerifyPaymentResponse
{
    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public bool IsPaid { get; set; }
        public int OriginalAmountPaid { get; set; }
    }
}