namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;

public class GetMetaDataResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class MetadataList
    {
        public ReportDetails ReportDetails { get; set; }
        public List<Param> Params { get; set; }
    }

    public class Param
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public bool MultiValue { get; set; }
        public string DisplayText { get; set; }
    }

    public class ReportDetails
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<MetadataList> MetadataList { get; set; }
    }
}