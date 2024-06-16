namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;

public class ResponseV3Refund
{
    public class Root
    {
        public string RefundId { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public string Currency { get; set; }
        public double? NonCreditRefundAmount { get; set; }
        public double? CreditRefundAmount { get; set; }
        public string TraceId { get; set; }
        public Error Error { get; set; }
        public Summary Summary { get; set; }
    }

    public class Summary
    {
        public double? TotalAmount { get; set; }
        public double? FailedAmount { get; set; }
        public double? SucceededAmount { get; set; }
        public double? PendingAmount { get; set; }
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