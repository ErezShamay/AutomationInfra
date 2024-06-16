namespace Splitit.Automation.NG.Backend.Services.AdminApi.BusinessUnit.Responses;

public class GetListResponse
{
    public class BusinessUnit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
    }
}