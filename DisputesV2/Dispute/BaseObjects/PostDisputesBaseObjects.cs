namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;

public class PostDisputesBaseObjects
{
    public class PagingRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class Root
    {
        public DateTime? CreateAtFrom { get; set; }
        public DateTime? CreateAtTo { get; set; }
        public DateTime? ModifiedDateFrom { get; set; }
        public DateTime? ModifiedDateTo { get; set; }
        public int MerchantId { get; set; }
        public List<string> InstallmentPlanNumbers { get; set; }
        public PagingRequest PagingRequest { get; set; }
    }
}