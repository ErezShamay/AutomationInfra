namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;

public class DisputeResolveBaseObjects
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public List<string> TransactionIdToMarkAsWon { get; set; }
        public List<string> TransactionIdToMarkAsLost { get; set; }
    }
}