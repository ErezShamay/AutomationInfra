namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class GetAccountsPrintNodesIdResponse
{
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
        public TreeNode TreeNode { get; set; }
    }

    public class TreeNode
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public List<string> Children { get; set; }
    }
}