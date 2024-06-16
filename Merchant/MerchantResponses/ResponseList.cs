namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantResponses;

public class ResponseList
{
    public class AccountCurrency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
    }

    public class BankDetails
    {
        public string BankAccountName { get; set; }
        public string BankRoutingNumber { get; set; }
        public string BankAccountNumber { get; set; }
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

    public class BusinessContact
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
    }

    public class BusinessUnit
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

    public class FinancialContact
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
    }

    public class Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string SplititMerchantId { get; set; }
        public string CrmId { get; set; }
        public string MerchantVertical { get; set; }
        public string BusinessLegalName { get; set; }
        public string BusinessDBAName { get; set; }
        public int BusinessUnitId { get; set; }
        public int OnBoardingStatus { get; set; }
        public int? DeclaredAnnualVolumeUSD { get; set; }
        public int? DeclaredAOVUSD { get; set; }
        public string Vertical { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string LogoImageFileContent { get; set; }
        public string LogoImageFileExt { get; set; }
        public string LogoImageUrl { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public List<TransactionCurrency> TransactionCurrencies { get; set; }
        public List<AccountCurrency> AccountCurrencies { get; set; }
        public bool AccountCurrencyIsTransactionCurrencies { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public string VatTaxId { get; set; }
        public string RegisteredCountryOfBusinessCode { get; set; }
        public string RegisteredStateOfBusinessCode { get; set; }
        public string MerchantCountryCode { get; set; }
        public string MerchantStateCode { get; set; }
        public BankDetails BankDetails { get; set; }
        public string SplititCurrentEntity { get; set; }
        public string SplititSigningEntity { get; set; }
        public BusinessContact BusinessContact { get; set; }
        public TechnicalContact TechnicalContact { get; set; }
        public FinancialContact FinancialContact { get; set; }
        public string PayFacProcessId { get; set; }
        public string SalesForceAccountId { get; set; }
        public string SalesForceLeadId { get; set; }
        public string SalesForceAccountParentName { get; set; }
        public string SalesForceParentAccountId { get; set; }
        public bool IsSelfOnBoarding { get; set; }
        public string OnBoardingMethod { get; set; }
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
        public List<Merchant> Merchants { get; set; }
    }

    public class TechnicalContact
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
    }

    public class TransactionCurrency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
    }
}