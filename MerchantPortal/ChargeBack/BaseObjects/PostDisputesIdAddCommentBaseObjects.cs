namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.BaseObjects;

public class PostDisputesIdAddCommentBaseObjects
{
    public class Root
    {
        public string Text { get; set; }
        public string CreatedBy { get; set; }
        public string CommentId { get; set; }
        public bool InternalComment { get; set; }
    }
}