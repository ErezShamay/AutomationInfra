namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;

public class PostDisputeMarkBaseObjects
{
    public class Root
    {
        public List<string> TransactionIdsToMark { get; set; }
        public List<string> TransactionIdsToUnmark { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public bool PartialResponseMapping { get; set; }
    }
}