namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;

public class ResponseV3GetInstallmentPlan
{
    public class Authorization
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string SplititErrorResultCode { get; set; }
        public string GatewayTransactionID { get; set; }
        public string GatewayResultCode { get; set; }
        public string GatewayResultMessage { get; set; }
        public ThreeDSRedirect ThreeDSRedirect { get; set; }
        public string CAVV { get; set; }
        public string ECI { get; set; }
        public string GatewaySourceResponse { get; set; }
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

    public class BluesnapVaultedShopperToken
    {
        public string Token { get; set; }
        public string Last4Digit { get; set; }
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
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Installment
    {
        public int InstallmentNumber { get; set; }
        public int Amount { get; set; }
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

    public class MockerShopperToken
    {
        public string Token { get; set; }
        public string Last4Digit { get; set; }
    }

    public class Params
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class PaymentMethod
    {
        public string Type { get; set; }
        public Card Card { get; set; }
        public string Token { get; set; }
        public BluesnapVaultedShopperToken BluesnapVaultedShopperToken { get; set; }
        public MockerShopperToken MockerShopperToken { get; set; }
        public SpreedlyToken SpreedlyToken { get; set; }
    }

    public class Refund
    {
        public string RefundId { get; set; }
        public DateTime SubmitDate { get; set; }
        public int TotalAmount { get; set; }
        public string Status { get; set; }
        public int NonCreditRefundAmount { get; set; }
        public int CreditRefundAmount { get; set; }
    }

    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string RefOrderNumber { get; set; }
        public string PurchaseMethod { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public int OriginalAmount { get; set; }
        public int Amount { get; set; }
        public Authorization Authorization { get; set; }
        public Shopper Shopper { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ExtendedParams ExtendedParams { get; set; }
        public List<Installment> Installments { get; set; }
        public List<Refund> Refunds { get; set; }
        public Links Links { get; set; }
    }

    public class Shopper
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Culture { get; set; }
    }

    public class SpreedlyToken
    {
        public string Token { get; set; }
        public string Last4Digit { get; set; }
    }

    public class ThreeDSRedirect
    {
        public string Url { get; set; }
        public string Verb { get; set; }
        public Params Params { get; set; }
    }
}