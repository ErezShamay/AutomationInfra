namespace Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanBaseObjects;

public class CreateBaseObjectsBaseObjects
{
    public class Root
    {
        public string InstallmentPlanNumber { get; set; }
        public PlanData PlanData { get; set; }
        public CartData CartData { get; set; }
        public ConsumerData ConsumerData { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public CreditCardDetails CreditCardDetails { get; set; }
        public PaymentToken PaymentToken { get; set; }
        public PlanApprovalEvidence PlanApprovalEvidence { get; set; }
        public RedirectUrls RedirectUrls { get; set; }
        public EventsEndpoints EventsEndpoints { get; set; }
        public ExternalAuth ExternalAuth { get; set; }
    }
    
    public class Address
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string FullAddressLine { get; set; }
    }

    public class Amount
    {
        public int Value { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class AmountDetails
    {
        public int SubTotal { get; set; }
        public int Tax { get; set; }
        public int Shipping { get; set; }
    }

    public class BillingAddress
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string FullAddressLine { get; set; }
    }

    public class CardBrand
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CardType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CartData
    {
        public List<Item> Items { get; set; }
        public AmountDetails AmountDetails { get; set; }
    }

    public class ConsumerData
    {
        public string Id { get; set; }
        public string UniqueId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CultureName { get; set; }
        public string RoleName { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDataRestricted { get; set; }
        public bool IsDataPrivateRestricted { get; set; }
    }

    public class CreditCardDetails
    {
        public string CardId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public CardBrand CardBrand { get; set; }
        public CardType CardType { get; set; }
        public string Bin { get; set; }
        public string CardHolderFullName { get; set; }
        public string CardCvv { get; set; }
        public Address Address { get; set; }
        public string Token { get; set; }
    }

    public class EventsEndpoints
    {
        public string CreateSucceeded { get; set; }
    }

    public class ExtendedParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class ExternalAuth
    {
        public string UniqueGatewayAuthID { get; set; }
        public DateTime Date { get; set; }
        public Amount Amount { get; set; }
        public string TransactionFullLog { get; set; }
    }

    public class FirstInstallmentAmount
    {
        public int Value { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public Price Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

    public class PaymentToken
    {
        public string Token { get; set; }
        public string Type { get; set; }
        public string BillingData { get; set; }
        public string Last4Digits { get; set; }
    }

    public class PlanApprovalEvidence
    {
        public string CustomerSignaturePngAsBase64 { get; set; }
        public bool AreTermsAndConditionsApproved { get; set; }
        public DateTime ShopperApprovalDateTime { get; set; }
    }

    public class PlanData
    {
        public int NumberOfInstallments { get; set; }
        public Amount Amount { get; set; }
        public FirstInstallmentAmount FirstInstallmentAmount { get; set; }
        public string RefOrderNumber { get; set; }
        public string TestMode { get; set; }
        public string PurchaseMethod { get; set; }
        public string Strategy { get; set; }
        public ExtendedParams ExtendedParams { get; set; }
        public DateTime FirstChargeDate { get; set; }
        public bool AutoCapture { get; set; }
        public bool IsFunded { get; set; }
        public bool Attempt3DSecure { get; set; }
        public bool ExternalProviderSupported { get; set; }
    }

    public class Price
    {
        public int Value { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class RedirectUrls
    {
        public string Succeeded { get; set; }
        public string Canceled { get; set; }
        public string Failed { get; set; }
    }
}