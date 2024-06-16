using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Merchant.MerchantFunctionality;

public class UsersDeleteFunctionality
{
    private const string EndPoint = "/api/merchant/users/delete";
    private readonly HttpSender _httpSender = new();
    private readonly UsersDelete.Root _root = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponseUsersDelete.Root> SendPostRequestForUsersDeleteAsync(
        RequestHeader requestHeader, string userId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForUsersDeleteAsync");
            _root.UserId = userId;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.AdminUrl + EndPoint,
                _root, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseUsersDelete.Root>(response);
            Console.WriteLine("Done with SendPostRequestForUsersDeleteAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForUsersDeleteAsync\n" + exception);
            throw;
        }
    }
}