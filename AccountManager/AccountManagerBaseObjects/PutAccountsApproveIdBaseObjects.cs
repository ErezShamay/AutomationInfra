namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManager.AccountManagerBaseObjects;

public class PutAccountsApproveIdBaseObjects
{
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
        public List<string> BillingExecutionIndications { get; set; }
    }

    public class TouchPoint
    {
        public string Code { get; set; }
        public string Version { get; set; }
        public string SubVersion { get; set; }
        public int VersionedTouchpointId { get; set; }
    }
}