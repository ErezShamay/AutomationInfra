namespace Splitit.Automation.NG.Backend.Services.AdminApi.Disputes.DisputesResponses;

public class DisputeChargesResponse
{
    public class Dispute
    {
        public PlanInfo PlanInfo { get; set; }
        public int DisputedChargesMerchantInfoId { get; set; }
        public string Reason { get; set; }
        public double TotalAmount { get; set; }
        public int ProcessorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime MerchantDueDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Liability { get; set; }
        public List<DisputedCharge> DisputedCharges { get; set; }
        public string Status { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public int InstallmentPlanId { get; set; }
    }

    public class DisputedCharge
    {
        public int InstallmentId { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public string DisputeExternalId { get; set; }
        public DateTime DisputeDate { get; set; }
        public string Subject { get; set; }
        public string ShopperName { get; set; }
        public string MerchantName { get; set; }
    }

    public class PagingResponseHeader
    {
        public int TotalNumber { get; set; }
    }

    public class PlanInfo
    {
        public bool ArePaymentsOnHold { get; set; }
        public string InstallmentPlanNumber { get; set; }
        public string MerchantName { get; set; }
        public string ShopperName { get; set; }
        public string Strategy { get; set; }
        public string InstallmentPlanStatus { get; set; }
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public string TraceId { get; set; }
    }

    public class Root
    {
        public List<Dispute> Disputes { get; set; }
        public PagingResponseHeader PagingResponseHeader { get; set; }
        public ResponseHeader ResponseHeader { get; set; }
    }
}