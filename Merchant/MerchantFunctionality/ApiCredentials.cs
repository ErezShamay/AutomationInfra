using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;

public class ApiCredentials
{
    private const string MerchantApiCredentialsEndPoint = "/api/merchant/api-credentials";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponseApiCredentials.Root?> SendGetRequestForApiCredentialsAsync(RequestHeader requestHeader, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForApiCredentials");
            var endPoint = _envConfig.AdminUrl + MerchantApiCredentialsEndPoint + "?MerchantId=" + merchantId;
            var response = await _httpSender.SendGetHttpsRequestAsync(endPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseApiCredentials.Root>(response);
            Console.WriteLine("Done SendGetRequestForApiCredentials\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SendGetRequestForApiCredentials \n" + exception + "\n");
            return null;
        }
    }
}