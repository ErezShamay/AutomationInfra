using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class GetDisputesBulkFunctionality
{
    private const string EndPoint = "/api/v1/disputes/bulk";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetDisputesBulkResponse.Root> SendGetRequestGetDisputesBulkAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetDisputesBulk");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.DisputesV2Url + EndPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetDisputesBulkResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetDisputesBulk\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetCommentsDisputeId \n" + exception + "\n");
            throw;
        }
    }
}