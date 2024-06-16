namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanResponses;

public class GetInitiatedInstallmentPlanRequestResponse
{
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

    public class CurrencyDisplay
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public int DecimalPlaces { get; set; }
    }

    public class DisplayProperties
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ErrorIndicator
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ExtendedParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
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

    public class MerchantData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class PaymentFormMessage
    {
        public string Type { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsEmpty { get; set; }
    }

    public class PaymentWizardDataResponse
    {
        public string RequestedNumberOfInstallments { get; set; }
        public string SuccessExitURL { get; set; }
        public string ErrorExitURL { get; set; }
        public string CancelExitURL { get; set; }
        public string SuccessAsyncUrl { get; set; }
        public string ViewName { get; set; }
        public bool IsOpenedInIframe { get; set; }
        public bool Is3dSecureInPopup { get; set; }
        public string PaymentFormMessage { get; set; }
        public bool SetShortUrl { get; set; }
        public string ShowAddressElements { get; set; }
        public CurrencyDisplay CurrencyDisplay { get; set; }
        public bool ForceDisplayImportantNotes { get; set; }
        public bool ShowShopperDetailsExpendedOnStart { get; set; }
        public bool ShowPaymentScheduleRequiredCredit { get; set; }
        public bool IsShopperEmailMandatory { get; set; }
        public bool IsShopperPhoneMandatory { get; set; }
        public string NumberOfInstallmentsSelectionsOption { get; set; }
        public int InstallmentsScheduleDaysInterval { get; set; }
        public bool Is3ds2Supported { get; set; }
        public string ProcessorName { get; set; }
        public bool AddressIsReadonly { get; set; }
        public bool PhoneIsReadOnly { get; set; }
        public bool EmailIsReadOnly { get; set; }
        public bool ShowLearnMore { get; set; }
        public bool ShowMobilePhone { get; set; }
        public bool ShowCloseDialogBeforeAbandon { get; set; }
        public string LogoURL { get; set; }
        public int DefaultNumOfInstallments { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public string TermsAndConditionsUrl { get; set; }
        public string LearnMoreUrl { get; set; }
        public List<string> PotentialCardTypes { get; set; }
        public List<string> PotentialCardBrands { get; set; }
        public List<PaymentFormMessage> PaymentFormMessages { get; set; }
        public DisplayProperties DisplayProperties { get; set; }
        public List<string> PaymentMethods { get; set; }
        public string Status { get; set; }
        public bool IsAttempt3Dsecure { get; set; }
        public TermsAndConditions TermsAndConditions { get; set; }
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

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public PlanData PlanData { get; set; }
        public CartData CartData { get; set; }
        public ConsumerData ConsumerData { get; set; }
        public MerchantData MerchantData { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentWizardDataResponse PaymentWizardDataResponse { get; set; }
        public ErrorIndicator ErrorIndicator { get; set; }
    }

    public class Term
    {
        public string Type { get; set; }
        public string TranslationKey { get; set; }
        public string TranslationCategory { get; set; }
        public string TranslationHtmlContent { get; set; }
    }

    public class TermsAndConditions
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public List<Term> Terms { get; set; }
    }
}