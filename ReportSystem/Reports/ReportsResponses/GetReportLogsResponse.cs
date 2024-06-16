namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;

public class GetReportLogsResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class PagingResponseHeader
    {
        public int TotalNumber { get; set; }
    }

    public class Result
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ConfigId { get; set; }
        public string JobRunId { get; set; }
        public int MerchantId { get; set; }
        public string UserId { get; set; }
        public bool Success { get; set; }
        public DateTime RunTimeUTC { get; set; }
        public string FileUrl { get; set; }
        public int DeliveryMethod { get; set; }
        public int Format { get; set; }
        public string DataSourceId { get; set; }
        public string NotificationDestination { get; set; }
        public string Message { get; set; }
        public List<string> SelectedColumns { get; set; }
        public string TimeZone { get; set; }
        public string ReportName { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Result> Results { get; set; }
        public PagingResponseHeader PagingResponseHeader { get; set; }
    }
}