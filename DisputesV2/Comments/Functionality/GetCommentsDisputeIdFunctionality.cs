using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.Functionality;

public class GetCommentsDisputeIdFunctionality
{
    private const string EndPoint = "/api/v1/comments/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetCommentsDisputeIdResponse.Root> SendGetRequestGetCommentsDisputeIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetCommentsDisputeId");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.DisputesV2Url + EndPoint + disputeId, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetCommentsDisputeIdResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetCommentsDisputeId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetCommentsDisputeId \n" + exception + "\n");
            throw;
        }
    }
}