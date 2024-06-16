namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;

public class ResponseV3
{
    public class ResponseRoot
    {
        public string InstallmentPlanNumber { get; set; }
        public string TraceId { get; set; }
        public Error Error { get; set; }
        public DateTime DateCreated { get; set; }
        public string RefOrderNumber { get; set; }
        public string PurchaseMethod { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public double OriginalAmount { get; set; }
        public double Amount { get; set; }
        public Dictionary<string, string> ExtendedParams { get; set; }
        public Authorization Authorization { get; set; }
        public Shopper Shopper { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<Installment> Installments { get; set; }
        public Links Links { get; set; }
    }
    
    public class Error
    {
        public ExtraData ExtraData { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public object AdditionalInfo { get; set; }
    }

    public class ExtraData
    {
        public string GatewayErrorCode { get; set; }
        public string GatewayErrorDescription { get; set; }
    }
    
    public class Authorization
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string SplititErrorResultCode { get; set; }
        public string GatewayTransactionID { get; set; }
        public string GatewayResultCode { get; set; }
        public string GatewayResultMessage { get; set; }
        public ThreeDSRedirect ThreeDSRedirect { get; set; }
        public object CAVV { get; set; }
        public object ECI { get; set; }
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

    public class Card
    {
        public string CardHolderFullName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpYear { get; set; }
        public string CardExpMonth { get; set; }
        public string CardBrand { get; set; }
        public string CardType { get; set; }
    }

    public class Installment
    {
        public int InstallmentNumber { get; set; }
        public double Amount { get; set; }
        public DateTime ProcessDateTime { get; set; }
        public string Status { get; set; }
    }

    public class Links
    {
        public string Checkout { get; set; }
        public string LearnMore { get; set; }
        public string TermsConditions { get; set; }
        public string PrivacyPolicy { get; set; }
    }

    public class Params
    {
        public object md { get; set; }
        public object pareq { get; set; }
        public object termUrl { get; set; }
        public string internalRedirectionUrl { get; set; }
        public string issuerRedirectUrl { get; set; }
    }

    public class PaymentMethod
    {
        public string Type { get; set; }
        public Card Card { get; set; }
    }

    public class Shopper
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Culture { get; set; }
    }

    public class ThreeDSRedirect
    {
        public string Url { get; set; }
        public string Verb { get; set; }
        public Params Params { get; set; }
        public string InternalRedirectUrl { get; set; }
    }
}