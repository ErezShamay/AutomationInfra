namespace Splitit.Automation.NG.PaymentsSystem.SplititOperations.Responses;

public class GetPaymentFundingPlanDetailsResponse
{
    public class LoanDetails
    {
        public int fundedAmount { get; set; }
        public int paidAmount { get; set; }
        public int exchnageRate { get; set; }
        public int processedNumberOfInstallments { get; set; }
        public string totalCollected { get; set; }
        public int totalCollectedCharges { get; set; }
        public int totalCollectedRefundCreditShopper { get; set; }
        public int totalCollectedRefundDeduction { get; set; }
        public DateTime financedStartDate { get; set; }
    }

    public class PlanActivity
    {
        public string batchId { get; set; }
        public int activity { get; set; }
        public int activityAmount { get; set; }
        public DateTime activityCreateDate { get; set; }
        public DateTime invoiceCreateDate { get; set; }
        public int activityExchangeRate { get; set; }
        public int activityStatus { get; set; }
    }

    public class PlanDetail
    {
        public string originalPlanAmount { get; set; }
        public string currentPlanAmount { get; set; }
        public int numberOfInstallments { get; set; }
        public int chargeNumberOfInstallments { get; set; }
        public int totalChargedAmount { get; set; }
        public int fixedFee { get; set; }
        public int variableFee { get; set; }
        public int totalRefundCreditShopper { get; set; }
        public int totalRefundDeduction { get; set; }
        public int planOutstandingAmount { get; set; }
        public string currency { get; set; }
        public string installmentPlanNumber { get; set; }
        public int fundingStatus { get; set; }
        public int installmentPlanStatus { get; set; }
    }

    public class Root
    {
        public PlanDetail planDetail { get; set; }
        public LoanDetails loanDetails { get; set; }
        public List<PlanActivity> planActivities { get; set; }
    }
}