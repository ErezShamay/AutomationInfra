namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Infrastructure.InfrastructureResponses;

public class ValuesTimeZonesResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Result
    {
        public string TimeZoneCode { get; set; }
        public string TimeZoneId { get; set; }
        public string DisplayName { get; set; }
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