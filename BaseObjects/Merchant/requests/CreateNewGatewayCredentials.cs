namespace Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.requests;

public class CreateNewGatewayCredentials
{
    public class Root
    {
        public List<object> PaymentMethods { get; set; }
        public GatewayCredentials GatewayCredentials { get; set; }
    }

    public class GatewayCredentials
    {
        public int TerminalId { get; set; }
        public int Id { get; set; }
        public List<ProcessorAuthenticationParametersWithValue> ProcessorAuthenticationParametersWithValues { get; set; }
        public int InstallmentsProcessorType { get; set; }
    }

    public class ProcessorAuthenticationParametersWithValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }
    }
}