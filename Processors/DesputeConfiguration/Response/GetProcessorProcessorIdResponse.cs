namespace Splitit.Automation.NG.Backend.Services.Processors.DesputeConfiguration.Response;

public class GetProcessorProcessorIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Reason
    {
        public int Id { get; set; }
        public string ReasonCode { get; set; }
        public string DisputeLiability { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Reason> Reasons { get; set; }
    }
}