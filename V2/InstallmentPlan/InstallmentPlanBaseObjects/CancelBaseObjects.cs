namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanBaseObjects;

public class CancelBaseObjects
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public string RefundUnderCancelation { get; set; }
        public string CancelationReason { get; set; }
        public bool PartialResponseMapping { get; set; }
    }
}