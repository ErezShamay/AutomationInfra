namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;

public class GetPaymentFundingPlanResponse
{
    public class PaymentFundingPlanResponse
    {
        public string id { get; set; }
        public string merchantId { get; set; }
        public string merchantName { get; set; }
        public string installmentPlanNumber { get; set; }
        public int billingPlanAmount { get; set; }
        public int planOutstandingAmount { get; set; }
        public string currency { get; set; }
        public int fundingType { get; set; }
        public int fundingStatus { get; set; }
        public int paymentRecordActivity { get; set; }
        public DateTime planCreationDate { get; set; }
        public DateTime invoicedDate { get; set; }
        public DateTime financedStartDate { get; set; }
    }

    public class Root
    {
        public List<PaymentFundingPlanResponse> paymentFundingPlanResponses { get; set; }
        public int totalRecords { get; set; }
    }
}