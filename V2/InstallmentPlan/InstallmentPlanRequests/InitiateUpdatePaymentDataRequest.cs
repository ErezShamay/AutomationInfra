namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanRequests;

public class InitiateUpdatePaymentDataRequest
{
    public class RedirectUrl
    {
        public string Succeeded { get; set; }
        public string Canceled { get; set; }
        public string Failed { get; set; }
    }

    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public RedirectUrl RedirectUrl { get; set; }
    }
}