using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminPortal.BaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminPortal.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminPortal.MerchantKey.Functionality;

public class PostMerchantIdKeysFunctionality
{
    private const string EndPoint = "/api/v1/merchant/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostMerchantIdKeysResponse.Root> SendPostRequestPostMerchantIdKeysAsync(
        RequestHeader requestHeader, PostMerchantIdKeysBaseObjects.Root postMerchantIdKeysBaseObjects, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostMerchantIdKeysAsync");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.AdminPortalApi + EndPoint + merchantId + "/keys",
                postMerchantIdKeysBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostMerchantIdKeysResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostMerchantIdKeysAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostMerchantIdKeysAsync \n" + exception + "\n");
            throw;
        }
    }
}