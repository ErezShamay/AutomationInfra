namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanBaseObjects;

public class GetSchedulesBaseObjects
{
    public class Amount
    {
        public int Value { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public Amount Amount { get; set; }
        public string CardNumber { get; set; }
    }
}