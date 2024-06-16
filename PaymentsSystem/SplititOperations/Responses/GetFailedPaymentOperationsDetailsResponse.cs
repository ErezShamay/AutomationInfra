namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;

public class GetFailedPaymentOperationsDetailsResponse
{
    public class Root
    {
        public string activity { get; set; }
        public string sku { get; set; }
        public string payload { get; set; }
    }
}