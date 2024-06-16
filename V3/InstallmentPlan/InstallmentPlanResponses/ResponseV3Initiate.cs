namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;

public class ResponseV3Initiate
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public string RefOrderNumber { get; set; }
        public string PurchaseMethod { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public ExtendedParams ExtendedParams { get; set; }
        public Shopper Shopper { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public string CheckoutUrl { get; set; }
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

    public class ExtendedParams
    {
        public string ThreeDSExemption { get; set; }
    }

    public class Shopper
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Culture { get; set; }
    }
}