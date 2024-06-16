using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.requests;
using Splitit.Automation.NG.Backend.Services.AdminApi.BaseObjects.Merchant.response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;
using Assert = NUnit.Framework.Assert;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Terminal;

public class Save
{
    private const string CreateTerminalEndPoint = "/api/terminal/save";
    private readonly HttpSender _httpSender = new();
    private readonly GatewayCredentialsSave _gatewayCredentialsSave = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> CreateTerminalAndCredentialSettingsAsync(
        RequestHeader requestHeader, int businessUnitId, int gatewayId)
    {
        try
        {
            Console.WriteLine("\nStarting CreateTerminalAndCredentialSettings");
            var createNewTerminalBaseObject = new CreateNewTerminal.Root();
            var populatedCreateNewTerminalBaseObject = CreateTerminalBaseObject(createNewTerminalBaseObject, businessUnitId, gatewayId);
            var jResponse = await SendSaveTerminalSettingsRequestAsync(requestHeader, populatedCreateNewTerminalBaseObject);
            var terminalApiKey = jResponse.Terminal.ApiKey;
            var terminalId = jResponse.Terminal.Id;
            var createNewGatewayCredentialsBaseObject = new CreateNewGatewayCredentials.Root();
            var populatedCreateNewGatewayCredentialsBaseObject = _gatewayCredentialsSave.CreateGatewayCredentialsBaseObject(createNewGatewayCredentialsBaseObject, terminalId, gatewayId);
            var jResponseGatewayCredentials = await _gatewayCredentialsSave.SendSaveCredentialSettingsRequestAsync(requestHeader, populatedCreateNewGatewayCredentialsBaseObject);
            Console.WriteLine("validating if jResponseGatewayCredentials succeeded");
            Assert.That(jResponseGatewayCredentials.ResponseHeader.Succeeded, Is.True);
            Console.WriteLine("jResponseGatewayCredentials succeeded");
            Console.WriteLine("Done CreateTerminalAndCredentialSettings\n");
            return terminalApiKey;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreateTerminalAndCredentialSettings \n" + exception + "\n");
            return null!;
        }
    }

    private CreateNewTerminal.Root CreateTerminalBaseObject(CreateNewTerminal.Root createNewTerminalBaseObject, int businessUnitId, int gatewayId)
    {
        try
        {
            Console.WriteLine("\nStarting CreateTerminalBaseObject");
            createNewTerminalBaseObject.Id = -1;
            createNewTerminalBaseObject.CanCancelInstallmentsPlan = false;
            createNewTerminalBaseObject.ChargeBeforeShipping = true;
            createNewTerminalBaseObject.IsAddressRequired = true;
            createNewTerminalBaseObject.UseTestGateway = true;
            createNewTerminalBaseObject.GatewayId = gatewayId;
            createNewTerminalBaseObject.NumberOfAllowedDaysForRefund = "365";
            createNewTerminalBaseObject.PendingShipmentReminderDays = "30";
            createNewTerminalBaseObject.ContinueExistingPlanWithOriginalGateway = true;
            createNewTerminalBaseObject.AllIssueCardCountriesIsoCodes = true;
            createNewTerminalBaseObject.Name = "Automation-" + GuidGenerator.GenerateNewGuid();
            createNewTerminalBaseObject.MerchantName = "Automation-" + GuidGenerator.GenerateNewGuid();
            createNewTerminalBaseObject.IssueCardCountriesIsoCodes = new List<object>();
            createNewTerminalBaseObject.BusinessUnitId = businessUnitId;
            createNewTerminalBaseObject.TestMode = "Regular";
            createNewTerminalBaseObject.Email = "splitit.automation@splitit.com";
            Console.WriteLine("Done CreateTerminalBaseObject\n");
            return createNewTerminalBaseObject;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreateTerminalBaseObject \n" + exception + "\n");
            return null!;
        }
    }

    private async Task<CreateNewTerminalResponse.Root> SendSaveTerminalSettingsRequestAsync(RequestHeader requestHeader, object obj)
    {
        try
        {
            Console.WriteLine("ֿ\nStarting SendSaveTerminalSettingsRequest");
            var endPoint = _envConfig.AdminUrl + CreateTerminalEndPoint;
            var response = await _httpSender.SendPostHttpsRequestAsync(endPoint, obj, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<CreateNewTerminalResponse.Root>(response);
            Console.WriteLine("Done SendSaveTerminalSettingsRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendSaveTerminalSettingsRequest \n" + exception + "\n");
            return null!;
        }
    }
}