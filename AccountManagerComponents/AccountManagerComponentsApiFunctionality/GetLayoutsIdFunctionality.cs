using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsApiFunctionality;

public class GetLayoutsIdFunctionality
{
    private const string EndPoint = "/api/v1/Layouts/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetLayoutsIdResponse.Root> SendGetRequestGetLayoutsId(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetLayoutsId");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.Ams + EndPoint, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetLayoutsIdResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetLayoutsId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetLayoutsId \n" + exception + "\n");
            throw;
        }
    }
}