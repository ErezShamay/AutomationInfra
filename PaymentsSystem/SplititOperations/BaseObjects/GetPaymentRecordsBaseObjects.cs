namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.BaseObjects;

public class GetPaymentRecordsBaseObjects
{
    public class Root
    {
        public int numberOfRowsInPage { get; set; }
        public int pageNumber { get; set; }
        public List<int> activities { get; set; }
        public string merchantId { get; set; }
        public string? installmentPlanNumber { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public List<int> paymentRecordStatus { get; set; }
        public string batchId { get; set; }
    }
}