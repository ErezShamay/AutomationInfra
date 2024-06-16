namespace Splitit.Automation.NG.Backend.Services.Processors.UnclassifiedGatewayErrors.Response;

public class GetProcessorProcessorIdResponse
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class Parameter
    {
        public int Id { get; set; }
        public int ProcessorId { get; set; }
        public string GatewayErrorCode { get; set; }
        public string GatewayErrorMessage { get; set; }
        public int Appearances { get; set; }
        public List<string> InstallmentPlanNumbers { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}