using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class GetDisputesDisputesIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetDisputesDisputesIdResponse.Root> SendGetRequestGetDisputesDisputesIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetDisputesDisputesId");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.DisputesV2Url + EndPoint + disputeId, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetDisputesDisputesIdResponse.Root>(response);
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