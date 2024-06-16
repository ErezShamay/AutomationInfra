using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyBaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyFunctionality;

public class PostKeyFunctionality
{
    private const string EndPoint = "/api/v1/key";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostKeyResponse.Root> SendPostRequestPostKeyAsync(
        RequestHeader requestHeader, PostKeyBaseObjects.Root postKeyBaseObjects)
    {
        var counter = 0;
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostKey");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint, 
                postKeyBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostKeyResponse.Root>(response);
            if (jResponse!.UniqueId == null!)
            {
                while (counter < 10)
                {
                    await Task.Delay(10 * 1000);
                    var responseInner = await _httpSender.SendPostHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint, 
                        postKeyBaseObjects, requestHeader);
                    var jResponseInner = JsonConvert.DeserializeObject<PostKeyResponse.Root>(responseInner);
                    if (jResponseInner!.UniqueId != null!)
                    {
                        return jResponseInner;
                    }
                    counter++;
                }
            }
            Console.WriteLine("Done with SendPostRequestPostKey\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostKey\n" + exception + "\n");
            throw;
        }
    }
}