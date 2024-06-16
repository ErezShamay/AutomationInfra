namespace Splitit.Automation.NG.Backend.Services.Ams.PartnerPublicIntegration.PartnerPublicIntegrationResponses;

public class StructureResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Field
    {
        public string AccountFieldName { get; set; }
        public string AccountFieldType { get; set; }
        public List<string> AllowedOptions { get; set; }
        public bool IsMandatory { get; set; }
    }

    public class MetaData
    {
        public List<Field> Fields { get; set; }
        public string Account { get; set; }
        public string RequestUrlEndpoint { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public string AuthorizationHeader { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<MetaData> MetaData { get; set; }
    }
}