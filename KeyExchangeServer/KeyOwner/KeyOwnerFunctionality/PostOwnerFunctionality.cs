using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.KeyOwner.KeyOwnerBaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.KeyOwner.KeyOwnerFunctionality;

public class PostOwnerFunctionality
{
    private const string EndPoint = "/api/v1/owner/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendPostRequestPostOwnerAsync(
        RequestHeader requestHeader, PostOwnerBaseObjects.Root postOwnerBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostOwnerAsync");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint, 
                postOwnerBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPostRequestPostOwnerAsync\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostOwnerAsync\n" + exception + "\n");
            throw;
        }
    }
}