namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetInfrastructureEnumsResponse
{
    public class AdditionalProp1
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class AdditionalProp2
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class AdditionalProp3
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class Enumeration
    {
        public List<AdditionalProp1> additionalProp1 { get; set; }
        public List<AdditionalProp2> additionalProp2 { get; set; }
        public List<AdditionalProp3> additionalProp3 { get; set; }
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
        public Enumeration Enumeration { get; set; }
    }
}