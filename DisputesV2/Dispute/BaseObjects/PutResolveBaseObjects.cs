namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;

public class PutResolveBaseObjects
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public List<int> InstallmentNumberToMarkAsWon { get; set; }
        public List<int> InstallmentNumberToMarkAsLost { get; set; }
    }
}