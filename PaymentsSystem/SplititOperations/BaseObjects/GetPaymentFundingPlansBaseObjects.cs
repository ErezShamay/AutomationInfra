namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.BaseObjects;

public class GetPaymentFundingPlansBaseObjects
{
    public class Root
    {
        public int numberOfRowsInPage { get; set; }
        public int pageNumber { get; set; }
        public List<int> activities { get; set; }
        public string merchantId { get; set; }
        public string installmentPlanNumber { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public List<int> fundingStatus { get; set; }
        public List<int> paymentRecordStatus { get; set; }
        public DateTime planCreationDateFrom { get; set; }
        public DateTime planCreationDateTo { get; set; }
        public DateTime financedStartDateFrom { get; set; }
        public DateTime financedStartDateTo { get; set; }
        public DateTime invoicedDateFrom { get; set; }
        public DateTime invoicedDateTo { get; set; }
        public string billingCurrency { get; set; }
        public int billngAmountFrom { get; set; }
        public int billngAmountTo { get; set; }
        public int outstandingAmount { get; set; }
    }
}