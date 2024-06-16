namespace Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.requests;

public class CreateNewTerminal
{
    public class Root
    {
        public int Id { get; set; }
        public bool CanCancelInstallmentsPlan { get; set; }
        public bool ChargeBeforeShipping { get; set; }
        public bool IsAddressRequired { get; set; }
        public bool UseTestGateway { get; set; }
        public int GatewayId { get; set; }
        public object CountriesCodes { get; set; }
        public string NumberOfAllowedDaysForRefund { get; set; }
        public string PendingShipmentReminderDays { get; set; }
        public bool ContinueExistingPlanWithOriginalGateway { get; set; }
        public bool AllIssueCardCountriesIsoCodes { get; set; }
        public object CurrencyCode { get; set; }
        public object CreatedDate { get; set; }
        public object IssueCardCountryName { get; set; }
        public string Name { get; set; }
        public object MerchantId { get; set; }
        public string MerchantName { get; set; }
        public List<object> IssueCardCountriesIsoCodes { get; set; }
        public object TerminalId { get; set; }
        public int BusinessUnitId { get; set; }
        public string TestMode { get; set; }
        public string Email { get; set; }
        public object IsEnabledToProcessDisputesInformation { get; set; }
    }
}