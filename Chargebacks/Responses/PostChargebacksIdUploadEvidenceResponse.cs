namespace Splitit.Automation.NG.Backend.Services.Chargebacks.Responses;

public class PostChargebacksIdUploadEvidenceResponse
{
    public class Evidence
    {
        public string EvidenceId { get; set; }
        public DateTime UploadedAt { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }

    public class Root
    {
        public Evidence Evidence { get; set; }
    }
}