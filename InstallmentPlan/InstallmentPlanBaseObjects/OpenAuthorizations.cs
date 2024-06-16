namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;

public class OpenAuthorizations
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public bool IsExecutedUnattended { get; set; }
    }
}