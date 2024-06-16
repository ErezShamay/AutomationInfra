using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;

public class GetDisputesDisputesIdCommentsFunctionality
{
    private const string EndPoint = "/api/v1/disputes/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetDisputesDisputesIdCommentsResponse.Root> SendGetRequestGetDisputesDisputesIdCommentsAsync(
        RequestHeader requestHeader, string disputeId, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetDisputesDisputesIdComments");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint + disputeId + "/comments", requestHeader, "yes", merchantId);
            var jResponse = JsonConvert.DeserializeObject<GetDisputesDisputesIdCommentsResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetDisputesDisputesIdComments\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetDisputesDisputesIdComments \n" + exception + "\n");
            throw;
        }
    }
}