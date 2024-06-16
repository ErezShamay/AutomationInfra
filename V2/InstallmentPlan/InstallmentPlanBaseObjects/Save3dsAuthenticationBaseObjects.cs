namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanBaseObjects;

public class Save3dsAuthenticationBaseObjects
{
    public class Root
    {
        public string FingerprintId { get; set; }
        public string AuthenticationResponseJson { get; set; }
        public string InstallmentPlanNumber { get; set; }
    }
}