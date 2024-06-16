namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.Responses;

public class PostCommentsDisputeIdResponse
{
    public class Comment
    {
        public string Text { get; set; }
        public DateTime CommentAt { get; set; }
        public string By { get; set; }
        public string CommentId { get; set; }
        public bool Internal { get; set; }
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