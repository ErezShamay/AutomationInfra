namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;

public class GetExtended
{
    public class DateInfo
    {
        public string InstallmentsPlanDateType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool AllDates { get; set; }
    }

    public class InitiatedStatuses
    {
        public bool ShowInitiatedPlansPaymentRequestSent { get; set; }
    }

    public class LoadRelated
    {
        public string Installments { get; set; }
        public string SecureAuthorizations { get; set; }
        public bool Disputes { get; set; }
        public bool Logos { get; set; }
    }

    public class PagingRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class QueryCriteria
    {
        // public int MerchantId { get; set; }
        // public string CurrencyCode { get; set; }
        // public int CurrencyId { get; set; }
        // public List<int> BusinessUnitIds { get; set; }
        // public string TerminalId { get; set; }
        // public int InstallmentPlanId { get; set; }
        public string InstallmentPlanNumber { get; set; }
        // public string RefOrderNumber { get; set; }
        // public int InstallmentPlanAmount { get; set; }
        // public string CardNumber { get; set; }
        // public string ConsumerName { get; set; }
        // public string ConsumerEmail { get; set; }
        // public string CardHolder { get; set; }
        // public int PisMemberId { get; set; }
        // public string PisMemberUniqueId { get; set; }
        // public string AnyFilter { get; set; }
        // public bool Eula { get; set; }
        // public bool ArePaymentsOnHold { get; set; }
        // public bool ShowChargebackPlans { get; set; }
        // public bool IsInAutoRetry { get; set; }
        // public List<string> Strategies { get; set; }
        // public InitiatedStatuses InitiatedStatuses { get; set; }
        // public string FraudCheckResult { get; set; }
        // public List<string> InstallmentsPlanStatuses { get; set; }
        // public List<string> TestModes { get; set; }
        // public List<string> DelayResolutions { get; set; }
        // public TransactionInformation TransactionInformation { get; set; }
        // public DateInfo DateInfo { get; set; }
    }

    public class Root
    {
        public Root(QueryCriteria queryCriteria)
        {
            QueryCriteria = queryCriteria;
        }

        public QueryCriteria QueryCriteria { get; set; }
        // public LoadRelated LoadRelated { get; set; }
        // public PagingRequest PagingRequest { get; set; }
    }

    public class TransactionInformation
    {
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
    }
}