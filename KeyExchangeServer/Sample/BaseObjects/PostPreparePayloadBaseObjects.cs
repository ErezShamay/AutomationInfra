namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;

public class PostPreparePayloadBaseObjects
{
    public class BillingAddress
    {
        public string AddressLine1 { get; set; }
        public string _AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class Card
    {
        public string CardHolderFullName { get; set; }
        public string CardNumber { get; set; }
        public int CardExpYear { get; set; }
        public int CardExpMonth { get; set; }
        public string CardCvv { get; set; }
    }

    public class ExtendedParams
    {
        public string ThreeDSExemption { get; set; }
        public string force3ds { get; set; }
        public string paymentRequestId { get; set; }
        public string externalOrderId { get; set; }
    }

    public class PaymentMethod
    {
        public string Type { get; set; }
        public Card Card { get; set; }
    }

    public class PlainTextPayload
    {
        public bool AttemptAuthorize { get; set; }
        public bool AutoCapture { get; set; }
        public bool Attempt3DSecure { get; set; }
        public bool TermsAndConditionsAccepted { get; set; }
        public PlanData PlanData { get; set; }
        public Shopper Shopper { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public RedirectUrls RedirectUrls { get; set; }
    }

    public class PlanData
    {
        public double TotalAmount { get; set; }
        public string Currency { get; set; }
        public int NumberOfInstallments { get; set; }
        public string PurchaseMethod { get; set; }
        public string RefOrderNumber { get; set; }
        public string _InstallmentPlanNumber { get; set; }
        public string TerminalId { get; set; }
        public double _FirstInstallmentAmount { get; set; }
        public string FirstInstallmentDate { get; set; }
        public ExtendedParams _ExtendedParams { get; set; }
    }

    public class RedirectUrls
    {
        public string AuthorizeSucceeded { get; set; }
        public string AuthorizeFailed { get; set; }
    }

    public class Root
    {
        public string KeyUniqueId { get; set; }
        public PlainTextPayload PlainTextPayload { get; set; }
    }

    public class Shopper
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Culture { get; set; }
    }
}