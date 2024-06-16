namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class PutAccountsApproveIdResponse
{
    public class AccountModel
    {
        public string Id { get; set; }
        public Company Company { get; set; }
        public FinancialInformation FinancialInformation { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public List<Contact> Contacts { get; set; }
        public RelatedEntities RelatedEntities { get; set; }
    }

    public class BillingInformation
    {
        public string LegalBusinessName { get; set; }
        public string Subsidiary { get; set; }
        public string VatGstNumber { get; set; }
        public string BillingCurrency { get; set; }
        public string MonetaryFlow { get; set; }
        public string RegisteredNumber { get; set; }
    }

    public class Company
    {
        public string AccountName { get; set; }
        public string AccountPhone { get; set; }
        public string AccountEmail { get; set; }
        public string Website { get; set; }
        public List<object> CompanyAddresses { get; set; }
    }

    public class Contact
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }

    public class CreditBank
    {
        public string SettlementChannel { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountType { get; set; }
        public string BankAccountName { get; set; }
        public string BSBCode { get; set; }
        public string TransitNumber { get; set; }
        public string BankCode { get; set; }
        public string SwiftCode { get; set; }
        public string IBAN { get; set; }
        public string BankNumber { get; set; }
    }

    public class DebitBank
    {
        public string SettlementChannel { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountType { get; set; }
        public string BankAccountName { get; set; }
        public string GoCardlessMandateId { get; set; }
        public string GoCardlessCustomerId { get; set; }
        public string GoCardlessGivenName { get; set; }
        public string GoCardlessFamilyName { get; set; }
        public string GoCardlessCompanyName { get; set; }
        public string GoCardlessEmail { get; set; }
        public string BSBCode { get; set; }
        public string BankNumber { get; set; }
    }

    public class FinancialInformation
    {
        public string UniqueId { get; set; }
        public BillingInformation BillingInformation { get; set; }
        public DebitBank DebitBank { get; set; }
        public CreditBank CreditBank { get; set; }
        public FundingSetup FundingSetup { get; set; }
        public List<object> Contracts { get; set; }
    }

    public class FundingSetup
    {
        public double CreditLine { get; set; }
        public string RiskRating { get; set; }
        public double ReservePool { get; set; }
        public string FundingTrigger { get; set; }
        public bool DebitOnHold { get; set; }
        public bool FundingOnHold { get; set; }
        public DateTime FundingEndDate { get; set; }
        public DateTime FundingStartDate { get; set; }
        public string SettlementType { get; set; }
        public string FundNonSecuredPlans { get; set; }
    }

    public class RelatedEntities
    {
    }

    public class Root
    {
        public AccountModel AccountModel { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
    }
}