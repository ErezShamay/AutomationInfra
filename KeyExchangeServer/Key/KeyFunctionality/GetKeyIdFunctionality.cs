using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyFunctionality;

public class GetKeyIdFunctionality
{
    private const string EndPoint = "/api/v1/key/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetKeyIdResponse.Root> SendGetRequestGetKeyIdAsync(
        RequestHeader requestHeader, string id)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetKeyId");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + 
                                                                 EndPoint + id, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetKeyIdResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetKeyId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetKeyId \n" + exception + "\n");
            throw;
        }
    }
}