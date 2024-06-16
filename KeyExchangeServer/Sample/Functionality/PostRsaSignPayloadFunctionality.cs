using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;

public class PostRsaSignPayloadFunctionality
{
    private const string EndPoint = "/api/v1/sample/rsa/sign-payload";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostRsaSignPayloadResponse.Root> SendPostRequestPostRsaSignPayloadAsync(
        RequestHeader requestHeader, PostRsaSignPayloadBaseObjects.Root postRsaSignPayloadBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostRsaSignPayloadAsync");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint, 
                postRsaSignPayloadBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostRsaSignPayloadResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostRsaSignPayloadAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostRsaSignPayloadAsync\n" + exception + "\n");
            throw;
        }
    }
}