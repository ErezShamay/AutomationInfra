namespace Splitit.Automation.NG.Backend.Services.V1._5.Login.LoginResponses;

public class LoginResponse
{
    public class Error
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public string additionalInfo { get; set; }
    }

    public class ResponseHeader
    {
        public bool succeeded { get; set; }
        public List<Error> errors { get; set; }
        public string traceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader responseHeader { get; set; }
        public string sessionId { get; set; }
    }
}