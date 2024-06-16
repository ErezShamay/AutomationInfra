namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;

public class GetPaymentRecordsResponse
{
    public class GetPaymentRecordResponse
    {
        public string merchantId { get; set; }
        public string installmentPlanNumber { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public DateTime createdDate { get; set; }
        public string paymentRecordStatus { get; set; }
        public string activity { get; set; }
        public string sku { get; set; }
    }

    public class Root
    {
        public List<GetPaymentRecordResponse> getPaymentRecordResponses { get; set; }
        public int totalRecords { get; set; }
    }
}