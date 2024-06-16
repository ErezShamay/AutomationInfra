namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerResponses;

public class PostCommentsResponse
{
    public class Comment
    {
        public string SplititAccountId { get; set; }
        public string CommentData { get; set; }
        public string Type { get; set; }
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
        public Comment Comment { get; set; }
    }
}