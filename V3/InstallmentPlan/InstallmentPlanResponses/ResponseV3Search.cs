namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;

public class ResponseV3Search
{
    public class Authorization
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string SplititErrorResultCode { get; set; }
        public string GatewayTransactionID { get; set; }
        public string GatewayResultCode { get; set; }
        public string GatewayResultMessage { get; set; }
        public object ThreeDSRedirect { get; set; }
        public string CAVV { get; set; }
        public string ECI { get; set; }
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
        public string CardCvv { get; set; }
        public string CardBrand { get; set; }
        public string CardType { get; set; }
    }

    public class ExtendedParams
    {
        public string IPayId { get; set; }
    }

    public class Installment
    {
        public int InstallmentNumber { get; set; }
        public double Amount { get; set; }
        public DateTime ProcessDateTime { get; set; }
        public string Status { get; set; }
    }

    public class PaymentMethod
    {
        public string Type { get; set; }
        public Card Card { get; set; }
    }

    public class PlanList
    {
        public string InstallmentPlanNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string RefOrderNumber { get; set; }
        public string PurchaseMethod { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public double OriginalAmount { get; set; }
        public double Amount { get; set; }
        public Authorization Authorization { get; set; }
        public Shopper Shopper { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ExtendedParams ExtendedParams { get; set; }
        public List<Installment> Installments { get; set; }
        public List<object> Refunds { get; set; }
        public object Links { get; set; }
    }

    public class Root
    {
        public List<PlanList> PlanList { get; set; }
    }

    public class Shopper
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Culture { get; set; }
    }
}