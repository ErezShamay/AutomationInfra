namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanBaseObjects;

public class ApproveBaseObjects
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public string CustomerSignaturePngAsBase64 { get; set; }
        public bool AreTermsAndConditionsApproved { get; set; }
    }
}