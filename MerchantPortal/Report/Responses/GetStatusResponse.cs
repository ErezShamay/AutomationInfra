namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;

public class GetStatusResponse
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
        public List<Status> Statuses { get; set; }
    }

    public class Status
    {
        public string status { get; set; }
        public int Count { get; set; }
    }
}