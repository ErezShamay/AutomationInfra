namespace Splitit.Automation.NG.Backend.Services.Ams.Internal.InternalBaseObjects;

public class ContractsBaseObjects
{
    public class Contract
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Pricing> Pricings { get; set; }
        public string SubscriptionERP_Id { get; set; }
        public string BusinessUnitUniqueId { get; set; }
    }

    public class Pricing
    {
        public string Id { get; set; }
        public string SKU { get; set; }
        public int TransactionFeePercentage { get; set; }
        public int TransactionFixedFee { get; set; }
        public int ChargebackFee { get; set; }
        public string ErpId { get; set; }
        public int BankRejectFee { get; set; }
    }

    public class RequestHeader
    {
        public TouchPoint TouchPoint { get; set; }
        public string SessionId { get; set; }
        public string ApiKey { get; set; }
        public string CultureName { get; set; }
        public string AuthenticationType { get; set; }
    }

    public class Root
    {
        public RequestHeader RequestHeader { get; set; }
        public List<Contract> Contracts { get; set; }
    }

    public class TouchPoint
    {
        public string Code { get; set; }
        public string Version { get; set; }
        public string SubVersion { get; set; }
        public int VersionedTouchpointId { get; set; }
    }
}