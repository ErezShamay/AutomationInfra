using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;

public class GetDisputesDisputesIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetDisputesDisputesIdResponse.Root> SendGetRequestGetDisputesDisputesIdAsync(
        RequestHeader requestHeader, string disputeId, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetDisputesDisputesId");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint + disputeId, requestHeader, "yes", merchantId);
            var jResponse = JsonConvert.DeserializeObject<GetDisputesDisputesIdResponse.Root>(response);
            if (jResponse!.Dispute.Id == null!)
            {
                for (var i = 0; i < 10; i++)
                {
                    await Task.Delay(5 * 1000);
                    response = await _httpSender.SendGetHttpsRequestAsync(
                        _envConfig.MerchantApiUrl + EndPoint + disputeId, requestHeader, "yes", merchantId);
                    jResponse = JsonConvert.DeserializeObject<GetDisputesDisputesIdResponse.Root>(response);
                    if (jResponse!.Dispute.Id != null!)
                    {
                        return jResponse;
                    }
                }
            }
            Console.WriteLine("Done with SendGetRequestGetDisputesDisputesId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetDisputesDisputesId \n" + exception + "\n");
            throw;
        }
    }
}