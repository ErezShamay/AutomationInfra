using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanBaseObjects;

public class RequestPaymentBaseObjects
{
    public class Root
    {
        public RequestHeader requestHeader { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public string PaymentApprovalPhone { get; set; }
        public string PaymentApprovalEmail { get; set; }
    }
}