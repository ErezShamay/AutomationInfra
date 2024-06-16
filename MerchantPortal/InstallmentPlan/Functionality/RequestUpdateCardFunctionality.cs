using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.InstallmentPlan.Functionality;

public class RequestUpdateCardFunctionality
{
    private const string EndPoint = "/api/v1/plan/";
    private readonly RequestUpdateCardBaseObjects.Root _root = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<RequestUpdateCardResponse.Root> SendPostRequestRequestUpdateCardAsync(
        RequestHeader requestHeader, string ipn, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestRequestUpdateCardAsync");
            var route = _envConfig.MerchantApiUrl + EndPoint + ipn + "/payment-data/request-update-card";
            var response = await _httpSender.SendPostHttpsRequestAsync(route,
                _root, requestHeader, null!, null!, "yes", merchantId);
            var jResponse = JsonConvert.DeserializeObject<RequestUpdateCardResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestRequestUpdateCardAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestRequestUpdateCardAsync\n" + exception + "\n");
            throw;
        }
    }
}