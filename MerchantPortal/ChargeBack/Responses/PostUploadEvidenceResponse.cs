namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Responses;

public class PostUploadEvidenceResponse
{
    public class Comment
    {
        public string Text { get; set; }
        public DateTime CommentAt { get; set; }
        public string By { get; set; }
        public string CommentId { get; set; }
        public bool Internal { get; set; }
    }

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
        public List<Evidence> Evidences { get; set; }
        public List<Comment> Comments { get; set; }
        public bool HasMoreChargebacks { get; set; }
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
        public string FileSize { get; set; }
        public bool Internal { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Dispute Dispute { get; set; }
    }
}