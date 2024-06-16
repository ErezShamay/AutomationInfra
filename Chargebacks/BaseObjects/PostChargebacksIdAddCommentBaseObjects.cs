namespace Splitit.Automation.NG.Backend.Services.Chargebacks.BaseObjects;

public class PostChargebacksIdAddCommentBaseObjects
{
    public class Root
    {
        public string Text { get; set; }
        public string CreatedBy { get; set; }
        public string CommentId { get; set; }
    }
}