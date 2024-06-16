namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;

public class PutMarkBaseObjects
{
    public class AdditionalData
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class InstallmentPlanLogItem
    {
        public int InstallmentPlanId { get; set; }
        public int LogId { get; set; }
        public string PaymentGatewayTransactionLog { get; set; }
        public int InstallmentId { get; set; }
        public string InstallmentPlanNumber { get; set; }
    }

    public class InternalDisputeNotification
    {
        public string UniqueId { get; set; }
        public string MessageType { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string GatewayTransactionId { get; set; }
        public DateTime DisputeCreatedDate { get; set; }
        public DateTime DisputeUntilDate { get; set; }
        public int GatewayId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DisputeDate { get; set; }
        public int Amount { get; set; }
        public string DisputeStatus { get; set; }
        public string TraceId { get; set; }
        public string Subject { get; set; }
        public AdditionalData AdditionalData { get; set; }
        public DateTime EvidenceSentDate { get; set; }
        public string Reason { get; set; }
        public bool IsEvidenceProvided { get; set; }
        public InstallmentPlanLogItem InstallmentPlanLogItem { get; set; }
    }

    public class Root
    {
        public List<InternalDisputeNotification> InternalDisputeNotifications { get; set; }
    }
}