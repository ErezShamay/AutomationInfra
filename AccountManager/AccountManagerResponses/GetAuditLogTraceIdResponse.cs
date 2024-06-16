namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetAuditLogTraceIdResponse
{
    public class ConnectionInfo
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Data
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Result
    {
        public DateTime CreatedAt { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Type { get; set; }
        public string TraceId { get; set; }
        public string PairId { get; set; }
        public string Uri { get; set; }
        public Data Data { get; set; }
        public ConnectionInfo ConnectionInfo { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Result> Result { get; set; }
    }
}