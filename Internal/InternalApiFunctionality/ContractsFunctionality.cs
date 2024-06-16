using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.Internal.InternalBaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.Internal.InternalApiFunctionality;

public class ContractsFunctionality
{
    private const string ContractsEndPoint = "/api/v1/accounts/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<string> SendPostRequestForContracts(
        ContractsBaseObjects.Root createAccountProfileIdBaseObjects, string accountId, RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForContracts");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.Ams + ContractsEndPoint + "/" + accountId + "/contracts", 
                createAccountProfileIdBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPostRequestForContracts\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForContracts \n" + exception + "\n");
            throw;
        }
    }
}