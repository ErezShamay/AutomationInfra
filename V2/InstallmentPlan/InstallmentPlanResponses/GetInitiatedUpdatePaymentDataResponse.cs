namespace Splitit.Automation.NG.Backend.Services.V2.InstallmentPlan.InstallmentPlanResponses;

public class GetInitiatedUpdatePaymentDataResponse
{
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

    public class CardBrand
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CardData
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

    public class CardType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class LastError
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Merchant
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class OutstandingAmount
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

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public CardData CardData { get; set; }
        public Merchant Merchant { get; set; }
        public RedirectUrls RedirectUrls { get; set; }
        public OutstandingAmount OutstandingAmount { get; set; }
        public TermsAndConditions TermsAndConditions { get; set; }
        public string ProcessorName { get; set; }
        public bool Is3DSRequired { get; set; }
        public LastError LastError { get; set; }
        public string Logo { get; set; }
        public string InstallmentPlanNumber { get; set; }
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