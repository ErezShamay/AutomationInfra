namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.BaseObjects;

public class PostDisputesBaseObjects
{
    public class PagingRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class Root
    {
        public DateTime CreateAtFrom { get; set; }
        public DateTime CreateAtTo { get; set; }
        public DateTime ModifiedDateFrom { get; set; }
        public DateTime ModifiedDateTo { get; set; }
        public int MerchantId { get; set; }
        public int ProcessorId { get; set; }
        public List<string> InstallmentPlanNumbers { get; set; }
        public string DisputeStatus { get; set; }
        public string InternalStatus { get; set; }
        public string Disputeliability { get; set; }
        public string InstallmentPlanStatus { get; set; }
        public PagingRequest PagingRequest { get; set; }
        public string MerchantReferenceId { get; set; }
    }
}