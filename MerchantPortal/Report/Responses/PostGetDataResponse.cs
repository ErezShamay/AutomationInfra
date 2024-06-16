namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;

public class PostGetDataResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class File
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string ContentBytes { get; set; }
        public string ContentString { get; set; }
        public string Url { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public File File { get; set; }
    }
}