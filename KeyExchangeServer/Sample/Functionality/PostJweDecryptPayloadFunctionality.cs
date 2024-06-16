using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;

public class PostJweDecryptPayloadFunctionality
{
    private const string EndPoint = "/api/v1/sample/jwe/decrypt-payload";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostJweDecryptPayloadResponse.Root> SendPostRequestPostJweDecryptPayloadAsync(
        RequestHeader requestHeader, PostJweDecryptPayloadBaseObjects.Root postJweDecryptPayloadBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostJweDecryptPayloadAsync");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint, 
                postJweDecryptPayloadBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostJweDecryptPayloadResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostJweDecryptPayloadAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostJweDecryptPayloadAsync\n" + exception + "\n");
            throw;
        }
    }
}