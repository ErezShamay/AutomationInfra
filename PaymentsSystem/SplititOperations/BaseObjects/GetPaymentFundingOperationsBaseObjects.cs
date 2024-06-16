namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.BaseObjects;

public class GetPaymentFundingOperationsBaseObjects
{
    public class Root
    {
        public string id { get; set; }
        public int numberOfRowsInPage { get; set; }
        public int pageNumber { get; set; }
        public string merchantId { get; set; }
        public string? installmentPlanNumber { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string batchId { get; set; }
        public string paymentRecordId { get; set; }
        public List<int> activities { get; set; }
        public List<int> paymentRecordStatus { get; set; }
        public List<int> convertionPhases { get; set; }
        public List<int> planFundingStatus { get; set; }
        public List<int> fundingType { get; set; }
        public List<int> settlementTypes { get; set; }
        public DateTime financedStartDateFrom { get; set; }
        public DateTime financedStartDateTo { get; set; }
        public DateTime invoicedDateFrom { get; set; }
        public DateTime invoicedDateTo { get; set; }
        public string billingCurrency { get; set; }
        public int outstandingAmountFrom { get; set; }
        public int outstandingAmountTo { get; set; }
        public string sortParam { get; set; }
        public bool isBusinessEx { get; set; }
    }
}