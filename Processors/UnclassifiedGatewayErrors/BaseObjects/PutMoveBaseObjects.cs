namespace Splitit.Automation.NG.Backend.Services.Processors.UnclassifiedGatewayErrors.BaseObjects;

public class PutMoveBaseObjects
{
    public class Root
    {
        public string GatewayErrorCode { get; set; }
        public string GatewayErrorMessage { get; set; }
        public int PisException { get; set; }
    }
}