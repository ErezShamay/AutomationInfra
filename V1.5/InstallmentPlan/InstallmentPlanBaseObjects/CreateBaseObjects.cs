using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;

public class CreateBaseObjects
{
    public class Address
    {
        public string addressLine { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string fullAddressLine { get; set; }
    }

    public class Amount
    {
        public int value { get; set; }
        public string currencyCode { get; set; }
    }

    public class AmountDetails
    {
        public int subTotal { get; set; }
        public int tax { get; set; }
        public int shipping { get; set; }
    }

    public class BillingAddress
    {
        public string addressLine { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string fullAddressLine { get; set; }
    }

    public class CardBrand
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class CardType
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class CartData
    {
        public List<Item> items { get; set; }
        public AmountDetails amountDetails { get; set; }
    }

    public class ConsumerData
    {
        public string id { get; set; }
        public string uniqueId { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string cultureName { get; set; }
        public bool isLocked { get; set; }
        public bool isDataRestricted { get; set; }
        public bool isDataPrivateRestricted { get; set; }
    }

    public class CreditCardDetails
    {
        public string cardId { get; set; }
        public string cardNumber { get; set; }
        public string cardExpMonth { get; set; }
        public string cardExpYear { get; set; }
        public CardBrand cardBrand { get; set; }
        public CardType cardType { get; set; }
        public string bin { get; set; }
        public string cardHolderFullName { get; set; }
        public string cardCvv { get; set; }
        public Address address { get; set; }
        public string token { get; set; }
    }

    public class EventsEndpoints
    {
        public string createSucceeded { get; set; }
    }

    public class ExtendedParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class ExternalAuth
    {
        public string uniqueGatewayAuthID { get; set; }
        public DateTime date { get; set; }
        public Amount amount { get; set; }
        public string transactionFullLog { get; set; }
    }

    public class FirstInstallmentAmount
    {
        public int value { get; set; }
        public string currencyCode { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string sku { get; set; }
        public Price price { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
    }

    public class PaymentToken
    {
        public string token { get; set; }
        public string type { get; set; }
        public string billingData { get; set; }
        public string last4Digits { get; set; }
    }

    public class PlanApprovalEvidence
    {
        public string customerSignaturePngAsBase64 { get; set; }
        public bool areTermsAndConditionsApproved { get; set; }
        public DateTime shopperApprovalDateTime { get; set; }
    }

    public class PlanData
    {
        public int numberOfInstallments { get; set; }
        public Amount amount { get; set; }
        public FirstInstallmentAmount firstInstallmentAmount { get; set; }
        public string refOrderNumber { get; set; }
        public int testMode { get; set; }
        public int purchaseMethod { get; set; }
        public int strategy { get; set; }
        public ExtendedParams extendedParams { get; set; }
        public DateTime firstChargeDate { get; set; }
        public Terminal terminal { get; set; }
        public bool autoCapture { get; set; }
        public bool isFunded { get; set; }
        public string attempt3DSecure { get; set; }
        public bool externalProviderSupported { get; set; }
    }

    public class Price
    {
        public int value { get; set; }
        public string currencyCode { get; set; }
    }

    public class RedirectUrls
    {
        public string succeeded { get; set; }
        public string canceled { get; set; }
        public string failed { get; set; }
    }

    public class RequestHeader
    {
        public TouchPoint touchPoint { get; set; }
        public string sessionId { get; set; }
        public string apiKey { get; set; }
        public string cultureName { get; set; }
        public int authenticationType { get; set; }
    }

    public class Root
    {
        public RequestHeader requestHeader { get; set; }
        public string installmentPlanNumber { get; set; }
        public PlanData planData { get; set; }
        public CartData cartData { get; set; }
        public ConsumerData consumerData { get; set; }
        public BillingAddress billingAddress { get; set; }
        public CreditCardDetails creditCardDetails { get; set; }
        public PaymentToken paymentToken { get; set; }
        public PlanApprovalEvidence planApprovalEvidence { get; set; }
        public RedirectUrls redirectUrls { get; set; }
        public EventsEndpoints eventsEndpoints { get; set; }
        public ExternalAuth externalAuth { get; set; }
    }

    public class Terminal
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }
}