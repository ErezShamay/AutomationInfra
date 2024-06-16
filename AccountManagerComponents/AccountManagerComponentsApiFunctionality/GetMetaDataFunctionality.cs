using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsApiFunctionality;

public class GetMetaDataFunctionality
{
    private const string EndPoint = "/api/v1/infrastructure/account/MetaData";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetMetaDataResponse.Root> SendGetRequestGetMetaData(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetMetaData");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.Ams + EndPoint, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetMetaDataResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetMetaData\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetMetaData \n" + exception + "\n");
            throw;
        }
    }
}