namespace Splitit.Automation.NG.Backend.Services.V2.Integration.IntegrationBaseObjects;

public class GatewayBaseObject
{
    public class Root
    {
        public IntegrationParams IntegrationParams { get; set; }
    }
    
    public class IntegrationParams
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }
}