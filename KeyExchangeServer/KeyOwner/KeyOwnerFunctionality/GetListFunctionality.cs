using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.KeyOwner.KeyOwnerResponse;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.KeyOwner.KeyOwnerFunctionality;

public class GetListFunctionality
{
    private const string EndPoint = "/api/v1/owner/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetListResponse.Root> SendGetRequestGetListAsync(
        RequestHeader requestHeader, string code)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetList");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + 
                                                                 EndPoint + code, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetListResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetList\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetList \n" + exception + "\n");
            throw;
        }
    }
}