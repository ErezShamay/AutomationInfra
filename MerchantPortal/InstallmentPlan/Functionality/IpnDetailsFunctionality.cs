using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.Functionality;

public class IpnDetailsFunctionality
{
    private const string EndPoint = "/api/v1/plan/";
    private readonly HttpSender _httpSender = new();
    private readonly IpnDetailsBaseObjects _ipnDetailsBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<IpnDetailsResponse.Root> SendPostRequestIpnDetailsAsync( 
        RequestHeader requestHeader, string ipn, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestUpdateInstallmentProcessDateTime");
            var route = _envConfig.MerchantApiUrl + EndPoint + ipn + "/details";
            var response = await _httpSender.SendPostHttpsRequestAsync(route,
                _ipnDetailsBaseObjects, requestHeader, null!, null!,
                "yes", merchantId);
            var jResponse = JsonConvert.DeserializeObject<IpnDetailsResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForMerchantReportsInsert\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestUpdateInstallmentProcessDateTime\n" + exception + "\n");
            throw;
        }
    }
}