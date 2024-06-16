namespace Splitit.Automation.NG.PaymentsSystem.PaymentsOperations.Responses;

public class BatchIdResponse
{
    public class Root
    {
        public List<string> pendingBatchIds { get; set; }
    }
}