namespace Splitit.Automation.NG.Backend.Services.DisputesV2.AuditLogs.Responses;

public class GetAuditLogsResponse
{
    public class AuditLog
    {
        public string Action { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string TraceId { get; set; }
        public string GatewayPayload { get; set; }
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
        public List<AuditLog> AuditLogs { get; set; }
        public int TotalResult { get; set; }
    }
}