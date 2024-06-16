using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.requests;
using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Terminal;

public class GatewayCredentialsSave
{
    private const string CreateGatewayCredentialsEndPoint = "/api/terminal/gateway-credentials/save";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public CreateNewGatewayCredentials.Root CreateGatewayCredentialsBaseObject(
        CreateNewGatewayCredentials.Root createNewGatewayCredentialsBaseObject, int terminalId, int gatewayId)
    {
        try
        {
            Console.WriteLine("\nStarting CreateGatewayCredentialsBaseObject");
            createNewGatewayCredentialsBaseObject.PaymentMethods = new List<object>();
            createNewGatewayCredentialsBaseObject.GatewayCredentials = new CreateNewGatewayCredentials.GatewayCredentials();
            if (gatewayId == 109)
            {
                createNewGatewayCredentialsBaseObject.PaymentMethods.Add("BluesnapVaultedShopperToken");
                createNewGatewayCredentialsBaseObject.PaymentMethods.Add("CreditCard");  
                createNewGatewayCredentialsBaseObject.GatewayCredentials.TerminalId = terminalId;
                createNewGatewayCredentialsBaseObject.GatewayCredentials.Id = gatewayId;
                createNewGatewayCredentialsBaseObject.GatewayCredentials.ProcessorAuthenticationParametersWithValues = new List<CreateNewGatewayCredentials.ProcessorAuthenticationParametersWithValue>();
                AddItemProcessorAuthenticationParametersWithValues(createNewGatewayCredentialsBaseObject.GatewayCredentials.ProcessorAuthenticationParametersWithValues, "Region", null!, "Region");
            }
            else
            {
                createNewGatewayCredentialsBaseObject.GatewayCredentials.TerminalId = terminalId;
                createNewGatewayCredentialsBaseObject.GatewayCredentials.Id = 44;
                createNewGatewayCredentialsBaseObject.GatewayCredentials.ProcessorAuthenticationParametersWithValues = new List<CreateNewGatewayCredentials.ProcessorAuthenticationParametersWithValue>();
                AddItemProcessorAuthenticationParametersWithValues(createNewGatewayCredentialsBaseObject.GatewayCredentials.ProcessorAuthenticationParametersWithValues, "user", "1", "User");
                AddItemProcessorAuthenticationParametersWithValues(createNewGatewayCredentialsBaseObject.GatewayCredentials.ProcessorAuthenticationParametersWithValues, "MerchantNum", "2", "Merchant Number");
                AddItemProcessorAuthenticationParametersWithValues(createNewGatewayCredentialsBaseObject.GatewayCredentials.ProcessorAuthenticationParametersWithValues, "pass", "3", "Password");
            }
            createNewGatewayCredentialsBaseObject.GatewayCredentials.InstallmentsProcessorType = 0;
            Console.WriteLine("Done CreateGatewayCredentialsBaseObject\n");
            return createNewGatewayCredentialsBaseObject;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreateGatewayCredentialsBaseObject \n" + exception + "\n");
            return null!;
        }
    }
    
    public async Task<CreateNewGatewayCredentialsResponse.Root> SendSaveCredentialSettingsRequestAsync(RequestHeader requestHeader, object obj)
    {
        try
        {
            Console.WriteLine("\nStarting SendSaveCredentialSettingsRequest");
            var endPoint = _envConfig.AdminUrl + CreateGatewayCredentialsEndPoint;
            var response = await _httpSender.SendPostHttpsRequestAsync(endPoint, obj, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<CreateNewGatewayCredentialsResponse.Root>(response);
            Console.WriteLine("Done SendSaveCredentialSettingsRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendSaveCredentialSettingsRequest \n" + exception + "\n");
            return null!;
        }
    }

    private void AddItemProcessorAuthenticationParametersWithValues(List<CreateNewGatewayCredentials.ProcessorAuthenticationParametersWithValue> processorAuthenticationParametersWithValuesList, 
        string key, string value, string displayName)
    {
        try
        {
            var processorAuthenticationParametersWithValue = new CreateNewGatewayCredentials.ProcessorAuthenticationParametersWithValue
                {
                    Key = key,
                    Value = value,
                    DisplayName = displayName
                };
            processorAuthenticationParametersWithValuesList.Add(processorAuthenticationParametersWithValue);
            Console.WriteLine("Added item to processorAuthenticationParametersWithValue list");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in AddItemProcessorAuthenticationParametersWithValues \n" + exception + "\n");
        }
    }
}