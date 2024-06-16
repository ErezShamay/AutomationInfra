using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;

public class List
{
    private const string SettingsEndPoint = "/api/merchant/list";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponseList.Root?> SendGetMerchantListRequestAsync(RequestHeader requestHeader, int businessUnitId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetMerchantListRequest");
            var endPoint = _envConfig.AdminUrl + SettingsEndPoint + "?MerchantQueryCriteria.BusinessUnitId=" + businessUnitId + "&IncludePaymentSettings=true";
            var response = await _httpSender.SendGetHttpsRequestAsync(endPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseList.Root>(response);
            Console.WriteLine("Done SendGetMerchantListRequest\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SendGetMerchantListRequest \n" + exception + "\n");
            return null;
        }
    }
}