namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetExternalNotificationsAccountIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Log
    {
        public string UniqueId { get; set; }
        public List<NotificationResult> NotificationResults { get; set; }
    }

    public class NotificationResult
    {
        public string ExecutionIndication { get; set; }
        public List<Error> Errors { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Log Log { get; set; }
    }
}