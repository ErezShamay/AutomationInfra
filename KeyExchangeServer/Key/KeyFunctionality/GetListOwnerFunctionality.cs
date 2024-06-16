using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyFunctionality;

public class GetListOwnerFunctionality
{
    private const string EndPoint = "/api/v1/key/list/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetKeyIdResponse.Root> SendGetRequestGetListOwnerAsync(
        RequestHeader requestHeader, string owner)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetListOwner");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + 
                                                                 EndPoint + owner, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetKeyIdResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetListOwner\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetListOwner \n" + exception + "\n");
            throw;
        }
    }
}