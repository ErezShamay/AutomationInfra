using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyBaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyFunctionality;

public class PostGenerateFunctionality
{
    private const string EndPoint = "/api/v1/key/generate";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostGenerateResponse.Root> SendPostRequestPostGenerateAsync(
        RequestHeader requestHeader, PostGenerateBaseObjects.Root postGenerateBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostPagesId");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint, 
                postGenerateBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostGenerateResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostPagesId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostPagesId\n" + exception + "\n");
            throw;
        }
    }
}