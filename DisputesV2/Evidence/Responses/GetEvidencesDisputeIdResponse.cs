namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Evidence.Responses;

public class GetEvidencesDisputeIdResponse
{
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
        public List<Evidence> Evidences { get; set; }
    }
}