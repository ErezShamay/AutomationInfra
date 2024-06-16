namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Infrastructure.InfrastructureResponses;

public class ValuesEnumsResponse
{
    public class AdditionalProp1
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class AdditionalProp2
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class AdditionalProp3
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Result
    {
        public List<AdditionalProp1> additionalProp1 { get; set; }
        public List<AdditionalProp2> additionalProp2 { get; set; }
        public List<AdditionalProp3> additionalProp3 { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Result Result { get; set; }
    }
}