namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;

public abstract class BaseObjectsInstallmentPlan
{
    public class installmentPlan
    {
        public BillingAddress BillingAddress = new();
        public PaymentMethod PaymentMethod = new();
        public PlanData planData = new();
        public RedirectUrls RedirectUrls = new();
        public Shopper Shopper = new();
        public bool AttemptAuthorize { get; set; }
        public bool AutoCapture { get; set; }
        public bool Attempt3dSecure { get; set; }
        public bool TermsAndConditionsAccepted { get; set; }
    }

    public class PlanData
    {
        public Dictionary<string, string> ExtendedParams = new();
        public double TotalAmount { get; set; }
        public string currency { get; set; }
        public int NumberOfInstallments { get; set; }
        public string PurchaseMethod { get; set; }
        public string RefOrderNumber { get; set; }
        public string TerminalId { get; set; }
        public int FirstInstallmentAmount { get; set; }
        public DateTime FirstInstallmentDate { get; set; }
    }

    public class Card
    {
        public string cardHolderFullName { get; set; }
        public string cardNumber { get; set; }
        public int cardExpYear { get; set; }
        public int cardExpMonth { get; set; }
        public string cardCvv { get; set; }
    }

    public class BillingAddress
    {
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    public class Shopper
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string culture { get; set; }
    }

    public class PaymentMethod
    {
        public Card card = new();
        public string type { get; set; }
    }

    public class RedirectUrls
    {
        public string authorizeSucceeded { get; set; }
        public string authorizeFailed { get; set; }
    }
}