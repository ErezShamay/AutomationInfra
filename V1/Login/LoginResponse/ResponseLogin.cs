namespace Splitit.Automation.NG.Backend.Services.V1.Login.LoginResponse;

public class ResponseLogin
{
    public class Root
    {
        public int Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public ResponseHeader ResponseHeader { get; set; }
        public string SessionId { get; set; }
    }
    
    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<object> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class ResponseStatus
    {
        public object ErrorCode { get; set; }
        public object Message { get; set; }
        public object StackTrace { get; set; }
        public object Errors { get; set; }
    }
}