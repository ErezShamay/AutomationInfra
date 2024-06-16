using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyFunctionality;

public class DeleteKeyIdFunctionality
{
    private const string EndPoint = "/api/v1/key/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<DeleteKeyIdResponse.Root> SendDeleteDeleteKeyIdAsync(
        RequestHeader requestHeader, string id)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeleteDeleteKeyId");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.Ams + EndPoint + id, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DeleteKeyIdResponse.Root>(response);
            Console.WriteLine("Done with SendDeleteDeleteKeyId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeleteDeleteKeyId \n" + exception + "\n");
            throw;
        }
    }
}