namespace Splitit.Automation.NG.Backend.Services.Chargebacks.Responses;

public class PostChargebacksIdAddCommentResponse
{
    public class Comment
    {
        public string CommentId { get; set; }
        public string Text { get; set; }
        public DateTime CommentAt { get; set; }
    }
    
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class Root
    {
        public Comment Comment { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Error> Errors { get; set; }
    }
}