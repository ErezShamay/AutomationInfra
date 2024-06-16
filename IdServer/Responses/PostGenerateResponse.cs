namespace Splitit.Automation.NG.Backend.Services.IdServer.responses;

public class PostGenerateResponse
{
    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public Secret Secret { get; set; }
        public string Password { get; set; }
    }

    public class Secret
    {
        public string UniqueId { get; set; }
        public string ClientId { get; set; }
        public string First4 { get; set; }
        public object ValidUntil { get; set; }
    }
}