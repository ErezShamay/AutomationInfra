namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;

public class CheckInstallmentsEligibilityBaseObjects
{
    public class Root
    {
        public PlanData PlanData { get; set; }
        public CardDetails CardDetails { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }
    
    public class BillingAddress
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class CardDetails
    {
        public string CardHolderFullName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpYear { get; set; }
        public string CardExpMonth { get; set; }
        public string CardCvv { get; set; }
        public string CardBrand { get; set; }
        public string CardType { get; set; }
    }

    public class PlanData
    {
        public string TerminalId { get; set; }
        public int TotalAmount { get; set; }
        public int FirstInstallmentAmount { get; set; }
        public string Currency { get; set; }
        public int NumberOfInstallments { get; set; }
        public string PurchaseMethod { get; set; }
        public string RefOrderNumber { get; set; }
        public Tags Tags { get; set; }
    }

    public class Tags
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }
}