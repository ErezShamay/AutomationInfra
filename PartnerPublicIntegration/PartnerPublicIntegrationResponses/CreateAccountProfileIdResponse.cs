namespace Splitit.Automation.NG.Backend.Services.Ams.PartnerPublicIntegration.PartnerPublicIntegrationResponses;

public class CreateAccountProfileIdResponse
{
    public class Data
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

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
        public Data Data { get; set; }
    }
}