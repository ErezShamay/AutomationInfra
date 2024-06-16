namespace Splitit.Automation.NG.Backend.Services.Ams.Workato.WorkatoBaseObjects;

public class ExternalNotificationsBaseObjects
{
    public class Root
    {
        public string AccountId { get; set; }
        public SyncCustomerResult SyncCustomerResult { get; set; }
        public List<SyncContractResult> SyncContractResult { get; set; }
        public List<SyncPricingResult> SyncPricingResult { get; set; }
        public SyncMerchantResult SyncMerchantResult { get; set; }
    }

    public class SyncContractResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string ContractId { get; set; }
        public string SubscriptionERP_Id { get; set; }
    }

    public class SyncCustomerResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string CustomerId { get; set; }
    }

    public class SyncMerchantResult
    {
        public string MerchantId { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
    }

    public class SyncPricingResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string ContractId { get; set; }
        public string PricingId { get; set; }
        public string PricingErpId { get; set; }
    }
}