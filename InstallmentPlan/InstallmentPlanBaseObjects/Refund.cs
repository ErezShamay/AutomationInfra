namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;

public class Refund
{
    public class Amount
    {
        public double Value { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public Amount Amount { get; set; }
        public string RefundStrategy { get; set; }
    }
}