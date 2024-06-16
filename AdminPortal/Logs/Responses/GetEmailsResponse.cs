namespace Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Responses;

public class GetEmailsResponse
{
    public class Attachment
    {
        public string FileUrl { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string FileManagerName { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Log
    {
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EmailType { get; set; }
        public string MessageProvider { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public string ExplicitRecipient { get; set; }
        public string ExplicitRecipientName { get; set; }
        public string TraceId { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public bool Result { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Log> Logs { get; set; }
    }
}