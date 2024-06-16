namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;

public class GenerateReportResponse
{
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
        public int DelliveredTo { get; set; }
        public string FileUrl { get; set; }
    }
}