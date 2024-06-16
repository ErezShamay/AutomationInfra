namespace Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.response;

public class CreateNewGatewayCredentialsResponse
{
    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public Terminal Terminal { get; set; }
    }
    
    public class BusinessUnit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Parameters
    {
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
        public string TraceId { get; set; }
    }

    public class Terminal
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ApiKey { get; set; }
        public string MerchantName { get; set; }
        public object TerminalId { get; set; }
        public bool ChargeBeforeShipping { get; set; }
        public bool CanCancelInstallmentsPlan { get; set; }
        public DateTime CreatedDate { get; set; }
        public double UtcOffset { get; set; }
        public string UtcOffsetString { get; set; }
        public object CurrencyCode { get; set; }
        public int Id { get; set; }
        public string TestMode { get; set; }
        public bool IsAddressRequired { get; set; }
        public bool UseTestGateway { get; set; }
        public int GatewayId { get; set; }
        public int NumberOfAllowedDaysForRefund { get; set; }
        public int PendingShipmentReminderDays { get; set; }
        public int BusinessUnitId { get; set; }
        public int HoursTillStartPlansDeletion { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public object IssueCardCountriesIsoCodes { get; set; }
        public bool AllIssueCardCountriesIsoCodes { get; set; }
        public Parameters Parameters { get; set; }
        public bool ContinueExistingPlanWithOriginalGateway { get; set; }
        public List<object> PaymentMethods { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsEnabledToProcessDisputesInformation { get; set; }
        public bool HasActiveTerminalGatewayData { get; set; }
    }
}