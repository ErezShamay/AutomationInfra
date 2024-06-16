namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;

public class GetBaseObjects
{
    public class DateInfo
    {
        public int installmentsPlanDateType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool allDates { get; set; }
    }

    public class InitiatedStatuses
    {
        public bool showInitiatedPlansPaymentRequestSent { get; set; }
    }

    public class LoadRelated
    {
        public int installments { get; set; }
        public int secureAuthorizations { get; set; }
        public bool disputes { get; set; }
        public bool logos { get; set; }
    }

    public class PagingRequest
    {
        public int skip { get; set; }
        public int take { get; set; }
    }

    public class QueryCriteria
    {
        public int merchantId { get; set; }
        public string currencyCode { get; set; }
        public int currencyId { get; set; }
        public List<int> businessUnitIds { get; set; }
        public string terminalId { get; set; }
        public int installmentPlanId { get; set; }
        public string installmentPlanNumber { get; set; }
        public string refOrderNumber { get; set; }
        public int installmentPlanAmount { get; set; }
        public string cardNumber { get; set; }
        public string consumerName { get; set; }
        public string consumerEmail { get; set; }
        public string cardHolder { get; set; }
        public int pisMemberId { get; set; }
        public string pisMemberUniqueId { get; set; }
        public string anyFilter { get; set; }
        public bool eula { get; set; }
        public bool arePaymentsOnHold { get; set; }
        public bool showChargebackPlans { get; set; }
        public bool isInAutoRetry { get; set; }
        public List<int> strategies { get; set; }
        public InitiatedStatuses initiatedStatuses { get; set; }
        public int fraudCheckResult { get; set; }
        public List<int> installmentsPlanStatuses { get; set; }
        public List<int> testModes { get; set; }
        public List<int> delayResolutions { get; set; }
        public TransactionInformation transactionInformation { get; set; }
        public DateInfo dateInfo { get; set; }
    }

    public class RequestHeader
    {
        public TouchPoint touchPoint { get; set; }
        public string sessionId { get; set; }
        public string apiKey { get; set; }
        public string cultureName { get; set; }
        public int authenticationType { get; set; }
    }

    public class Root
    {
        public QueryCriteria queryCriteria { get; set; }
        public LoadRelated loadRelated { get; set; }
        public PagingRequest pagingRequest { get; set; }
        public RequestHeader requestHeader { get; set; }
    }

    public class TouchPoint
    {
        public string code { get; set; }
        public string version { get; set; }
        public string subVersion { get; set; }
        public int versionedTouchpointId { get; set; }
    }

    public class TransactionInformation
    {
        public string transactionId { get; set; }
        public int transactionType { get; set; }
        public int transactionStatus { get; set; }
    }
}