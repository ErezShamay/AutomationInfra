namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;

public class PostDisputesResponse
{
    public class Dispute
    {
        public DateTime DisputeCreatedDate { get; set; }
        public string Id { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public string MerchantName { get; set; }
        public string Liabilty { get; set; }
        public string PlanStatus { get; set; }
        public string Amount { get; set; }
        public string Reason { get; set; }
        public DateTime MerchantDueDate { get; set; }
        public string MerchantReferenceId { get; set; }
        public string InternalStatus { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public int InstallmentNumber { get; set; }
        public string Accept { get; set; }
        public string ShopperName { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Dispute> Disputes { get; set; }
        public int TotalResult { get; set; }
    }
}