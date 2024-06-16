namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanBaseObjects;

public class RequestUpdateCardBaseObjects
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public string UpdateCardPhone { get; set; }
        public string UpdateCardEmail { get; set; }
    }
}