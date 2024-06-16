using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Credentials.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Credentials.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Credentials.Functionality;

public class PostGenerateFunctionality
{
    private const string EndPoint = "/api/v1/credentials/generate";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostGenerateResponse.Root> SendPostRequestPostGenerateAsync(
        RequestHeader requestHeader, PostGenerateBaseObjects.Root postGenerateBaseObjects, string? merchantPortalFlag, int merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostGenerateAsync");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.MerchantApiUrl + EndPoint, 
                postGenerateBaseObjects, requestHeader, null!, null!, merchantPortalFlag, merchantId.ToString());
            var jResponse = JsonConvert.DeserializeObject<PostGenerateResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostGenerateAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostGenerateAsync\n" + exception + "\n");
            throw;
        }
    }
}