namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.BaseObjects;

public class PostCommentsDisputeIdBaseObjects
{
    public class Root
    {
        public string? Text { get; set; }
        public string? CreatedBy { get; set; }
        public string? CommentId { get; set; }
        public bool? InternalComment { get; set; }
    }
}