namespace Splitit.Automation.NG.Backend.Services.Chargebacks.BaseObjects;

public class PostChargebacksBaseObjects
{
    public class Root
    {
        public DateTime ChargebackCreationDate { get; set; }
        public string RefDisputeId { get; set; }
        public DateTime DueDateForEvidence { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public string RefOrderNumber { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string ReasonCode { get; set; }
        public string TransactionSplititReference { get; set; }
    }
}