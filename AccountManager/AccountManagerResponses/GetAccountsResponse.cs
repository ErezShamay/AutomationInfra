namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetAccountsResponse
{
    public class Account
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
        public string Subsidiary { get; set; }
        public List<ExecutionIndication> ExecutionIndications { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ExecutionIndication
    {
        public string executionIndication { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Account> Accounts { get; set; }
        public int TotalResult { get; set; }
    }
}