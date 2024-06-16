namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class PutAccountsIdResponse
{
    public class AccountModel
    {
        public string Id { get; set; }
        public Company Company { get; set; }
        public FinancialInformation FinancialInformation { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Status { get; set; }
        public List<Contact> Contacts { get; set; }
        public object CustomerERPId { get; set; }
        public RelatedEntities RelatedEntities { get; set; }
        public object FileIds { get; set; }
        public string OwnerEmail { get; set; }
        public bool IsVirtual { get; set; }
    }

    public class BillingInformation
    {
        public string LegalBusinessName { get; set; }
        public string Subsidiary { get; set; }
        public object VatGstNumber { get; set; }
        public string BillingCurrency { get; set; }
        public string MonetaryFlow { get; set; }
        public string RegisteredNumber { get; set; }
    }

    public class Company
    {
        public string AccountName { get; set; }
        public object AccountPhone { get; set; }
        public string AccountEmail { get; set; }
        public string Website { get; set; }
        public List<CompanyAddress> CompanyAddresses { get; set; }
        public object EstimatedMSV { get; set; }
        public object BusinessStartDate { get; set; }
        public object MerchantCategoryCode { get; set; }
    }

    public class CompanyAddress
    {
        public string Country { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
    }

    public class Contact
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public object Note { get; set; }
        public object Phone { get; set; }
    }

    public class Contract
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public object EndDate { get; set; }
        public string SubscriptionERP_Id { get; set; }
        public List<Pricing> Pricings { get; set; }
        public string BusinessUnitUniqueId { get; set; }
    }

    public class CreditBank
    {
        public string SettlementChannel { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountType { get; set; }
        public string BankAccountName { get; set; }
        public object BSBCode { get; set; }
        public object TransitNumber { get; set; }
        public object BankCode { get; set; }
        public object SwiftCode { get; set; }
        public object IBAN { get; set; }
        public string BankNumber { get; set; }
        public object BankName { get; set; }
        public object BankAddress { get; set; }
    }

    public class DebitBank
    {
        public string SettlementChannel { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountType { get; set; }
        public string BankAccountName { get; set; }
        public string GoCardlessMandateId { get; set; }
        public object GoCardlessCustomerId { get; set; }
        public string GoCardlessGivenName { get; set; }
        public string GoCardlessFamilyName { get; set; }
        public string GoCardlessCompanyName { get; set; }
        public object GoCardlessEmail { get; set; }
        public object BSBCode { get; set; }
        public string BankNumber { get; set; }
    }

    public class FinancialInformation
    {
        public BillingInformation BillingInformation { get; set; }
        public DebitBank DebitBank { get; set; }
        public CreditBank CreditBank { get; set; }
        public FundingSetup FundingSetup { get; set; }
        public List<Contract> Contracts { get; set; }
        public string ExchangeRateProvider { get; set; }
    }

    public class FundingSetup
    {
        public double CreditLine { get; set; }
        public string RiskRating { get; set; }
        public object ReservePool { get; set; }
        public string FundingTrigger { get; set; }
        public bool DebitOnHold { get; set; }
        public bool FundingOnHold { get; set; }
        public object FundingEndDate { get; set; }
        public DateTime FundingStartDate { get; set; }
        public string SettlementType { get; set; }
        public string FundNonSecuredPlans { get; set; }
    }

    public class NotificationLogModel
    {
        public List<NotificationResult> NotificationResults { get; set; }
    }

    public class NotificationResult
    {
        public string ExecutionIndication { get; set; }
        public object Errors { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class Pricing
    {
        public string Id { get; set; }
        public string SKU { get; set; }
        public double TransactionFeePercentage { get; set; }
        public double TransactionFixedFee { get; set; }
        public double ChargebackFee { get; set; }
        public string ErpId { get; set; }
        public double BankRejectFee { get; set; }
        public string Description { get; set; }
        public double RefundReturnFee { get; set; }
        public double RefundFee { get; set; }
    }

    public class RelatedEntities
    {
        public object AMSParentId { get; set; }
        public object PartnerProfileId { get; set; }
    }

    public class Root
    {
        public AccountModel AccountModel { get; set; }
        public string FundingType { get; set; }
        public bool IsParent { get; set; }
        public bool HasAnyPartnerProfile { get; set; }
        public bool IsSyncedFully { get; set; }
        public bool FinancialDataExists { get; set; }
        public NotificationLogModel NotificationLogModel { get; set; }
        public object Signers { get; set; }
        public object Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
    }
}