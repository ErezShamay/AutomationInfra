namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;

public class Cancel
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public string RefundUnderCancelation { get; set; }
        public int CancelationReason { get; set; }
        public bool PartialResponseMapping { get; set; }
        public string Reference { get; set; }
    }
}