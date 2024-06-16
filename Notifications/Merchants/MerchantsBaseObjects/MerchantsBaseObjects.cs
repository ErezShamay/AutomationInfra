namespace Splitit.Automation.NG.Backend.Services.Notifications.Merchants.MerchantsBaseObjects;

public class MerchantsBaseObjects
{
    public class AdditionalParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

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

    public class BankDetails
    {
        public string bankAccountName { get; set; }
        public string bankRoutingNumber { get; set; }
        public string bankAccountNumber { get; set; }
    }

    public class Contact
    {
        public string type { get; set; }
        public string email { get; set; }
        public string fullName { get; set; }
    }

    public class MerchantSettingsExternalRequest
    {
        public int maxInstallments { get; set; }
        public int numberOfProducts { get; set; }
        public int numberOfNonSecureProducts { get; set; }
        public PaymentSettings paymentSettings { get; set; }
    }

    public class PaymentSettings
    {
        public string creditLine { get; set; }
        public string riskRating { get; set; }
        public string reservePool { get; set; }
        public string fundingTrigger { get; set; }
        public string debitOnHold { get; set; }
        public string fundingOnHold { get; set; }
        public string fundingEndDate { get; set; }
        public string fundingStartDate { get; set; }
        public string monetaryFlow { get; set; }
        public string settlementType { get; set; }
    }

    public class Root
    {
        public string sfAccountId { get; set; }
        public string email { get; set; }
        public string businessDBAName { get; set; }
        public string businessLegalName { get; set; }
        public string fullName { get; set; }
        public string technicalContactEmail { get; set; }
        public string sfLeadId { get; set; }
        public string sfParentAccountId { get; set; }
        public string sfParentAccountName { get; set; }
        public string tempMerchantId { get; set; }
        public string countryIsoCode3 { get; set; }
        public string websiteUrl { get; set; }
        public string subsidiary { get; set; }
        public string merchantVertical { get; set; }
        public string currencyIsoCode { get; set; }
        public List<Contact> contacts { get; set; }
        public Address address { get; set; }
        public BankDetails bankDetails { get; set; }
        public MerchantSettingsExternalRequest merchantSettingsExternalRequest { get; set; }
        public AdditionalParams additionalParams { get; set; }
        public string splititExternalId { get; set; }
        public string businessUnitExternalId { get; set; }
    }
}