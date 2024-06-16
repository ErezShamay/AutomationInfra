namespace Splitit.Automation.NG.Backend.Services.Processors.ErrorClassifications.Response;

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
        public string GatewayErrorCode { get; set; }
        public int PisException { get; set; }
        public bool AutoHandlingMode { get; set; }
        public bool CcNeedsUpdate { get; set; }
        public string GatewayErrorMessage { get; set; }
        public int ProcessorId { get; set; }
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