namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;

public class ResponsePlanJobFutureInformation
{
    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public List<FutureRun> FutureRuns { get; set; }
        public PagingResponseHeader PagingResponseHeader { get; set; }
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public string TraceId { get; set; }
    }
    
    public class FutureRun
    {
        public string ArgumentsMap { get; set; }
        public string Id { get; set; }
        public string CurrentState { get; set; }
        public string JobName { get; set; }
        public DateTime LastExecution { get; set; }
        public List<StateHistory> StateHistory { get; set; }
    }

    public class PagingResponseHeader
    {
        public int TotalNumber { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public List<object> Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class StateHistory
    {
        public int JobId { get; set; }
        public string Name { get; set; }
        public string RawData { get; set; }
        public DateTime Execution { get; set; }
        public string Reason { get; set; }
    }
    
    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public object AdditionalInfo { get; set; }
    }
}