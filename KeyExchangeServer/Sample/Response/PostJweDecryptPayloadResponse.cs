namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Response;

public class PostJweDecryptPayloadResponse
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
        public object CAVV { get; set; }
        public object ECI { get; set; }
        public string GatewaySourceResponse { get; set; }
    }

    public class BillingAddress
    {
        public string AddressLine { get; set; }
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

    public class ExactPayload
    {
        public string InstallmentPlanNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string RefOrderNumber { get; set; }
        public string PurchaseMethod { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public double OriginalAmount { get; set; }
        public double Amount { get; set; }
        public ExtendedParams ExtendedParams { get; set; }
        public Authorization Authorization { get; set; }
        public Shopper Shopper { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<Installment> Installments { get; set; }
        public Links Links { get; set; }
    }

    public class ExtendedParams
    {
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

    public class PaymentMethod
    {
        public string Type { get; set; }
        public Card Card { get; set; }
        public string Token { get; set; }
        public object BluesnapVaultedShopperToken { get; set; }
        public object MockerShopperToken { get; set; }
        public object SpreedlyToken { get; set; }
    }

    public class Root
    {
        public string Info { get; set; }
        public ExactPayload ExactPayload { get; set; }
    }

    public class Shopper
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Culture { get; set; }
    }
}