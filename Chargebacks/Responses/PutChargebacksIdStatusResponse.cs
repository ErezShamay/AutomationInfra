namespace Splitit.Automation.NG.Backend.Services.Chargebacks.Responses;

public class PutChargebacksIdStatusResponse
{
    public class Chargeback
    {
        public string ReasonCode { get; set; }
        public List<object> Evidences { get; set; }
        public List<object> Comments { get; set; }
        public string Id { get; set; }
        public DateTime DisputeCreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public int InstallmentNumber { get; set; }
        public string PlanStatus { get; set; }
        public string TransactionId { get; set; }
        public string MerchantName { get; set; }
        public TotalPlanAmount TotalPlanAmount { get; set; }
        public DisputeAmount DisputeAmount { get; set; }
        public string Status { get; set; }
        public DateTime PlanActivatedDate { get; set; }
    }

    public class DisputeAmount
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Root
    {
        public Chargeback Chargeback { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class TotalPlanAmount
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}