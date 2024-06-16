namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetAuditLogResponse
{
    public class AuditLog
    {
        public string SplititId { get; set; }
        public string ModifyBy { get; set; }
        public string Action { get; set; }
        public string TraceId { get; set; }
        public DateTime At { get; set; }
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
        public int Count { get; set; }
    }
}