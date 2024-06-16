namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;

public class GetFailedPaymentOperationsResponse
{
    public class Root
    {
        public List<object> getFailedPaymentOperationResponses { get; set; }
        public int totalRecords { get; set; }
    }
}