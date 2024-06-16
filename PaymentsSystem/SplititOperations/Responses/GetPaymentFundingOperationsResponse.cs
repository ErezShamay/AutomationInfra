namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;

public class GetPaymentFundingOperationsResponse
{
    public class PaymentFundingOperationResponse
    {
        public string merchantId { get; set; }
        public string settlementTypeDescription { get; set; }
        public string fundingCurrency { get; set; }
        public double fundedAmount { get; set; }
        public double paidAmount { get; set; }
        public int fundNumberOfActivities { get; set; }
        public double collectionAmount { get; set; }
        public int collectNumberOfActivities { get; set; }
        public double netByMerchant { get; set; }
        public bool doFailuresExist { get; set; }
        public bool isBusinessEx { get; set; }
        public List<int> activitiesList { get; set; }
    }

    public class Root
    {
        public List<PaymentFundingOperationResponse> paymentFundingOperationResponses { get; set; }
        public TotalCurrencies totalCurrencies { get; set; }
        public int totalRecords { get; set; }
    }

    public class TotalCurrencies
    {
        public USD USD { get; set; }
    }

    public class USD
    {
        public double totalFunds { get; set; }
        public double totalCollect { get; set; }
        public double totalNet { get; set; }
    }
}