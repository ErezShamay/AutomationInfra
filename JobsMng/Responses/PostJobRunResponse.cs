namespace Splitit.Automation.NG.Backend.Services.AdminApi.JobsMng.Responses;

public class PostJobRunResponse
{
    public class Root
    {
        public string Value { get; set; }
        public object Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
    }
}