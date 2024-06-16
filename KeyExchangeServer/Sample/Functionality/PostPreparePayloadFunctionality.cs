using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;

public class PostPreparePayloadFunctionality
{
    private const string EndPoint = "/api/v1/sample/jwe/prepare-payload";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostPreparePayloadResponse.Root> SendPostRequestPostPreparePayloadBaseObjectsAsync(
        RequestHeader requestHeader, PostPreparePayloadBaseObjects.Root postPreparePayloadBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostPreparePayloadBaseObjectsAsync");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint, 
                postPreparePayloadBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostPreparePayloadResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostPreparePayloadBaseObjectsAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostPreparePayloadBaseObjectsAsync\n" + exception + "\n");
            throw;
        }
    }
}