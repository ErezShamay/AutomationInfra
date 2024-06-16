namespace Splitit.Automation.NG.Backend.Services.Chargebacks.Responses;

public class PostChargebacksResponse
{
    public class Chargeback
    {
        public string Id { get; set; }
        public DateTime DisputeCreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public int InstallmentNumber { get; set; }
        public string PlanStatus { get; set; }
        public string TransactionId { get; set; }
        public string MerchantName { get; set; }
        public TotalPlanAmount TotalPlanAmount { get; set; }
        public DisputeAmount DisputeAmount { get; set; }
        public string Status { get; set; }
        public DateTime PlanActivatedDate { get; set; }
        public string ReasonCode { get; set; }
        public List<Evidence> Evidences { get; set; }
        public List<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public string CommentId { get; set; }
        public string Text { get; set; }
        public DateTime CommentAt { get; set; }
    }

    public class DisputeAmount
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Evidence
    {
        public string EvidenceId { get; set; }
        public DateTime UploadedAt { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Chargeback Chargeback { get; set; }
    }

    public class TotalPlanAmount
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}