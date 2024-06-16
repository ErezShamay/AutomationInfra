using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PutDueDateDisputeIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/due-date/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PutDueDateDisputeIdBaseObjects _putDueDateDisputeIdBaseObjects = new();
    
    public async Task<PutDueDateDisputeIdResponse.Root> SendPutRequestPutDueDateDisputeIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutDueDateDisputeId");
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint + disputeId,
                _putDueDateDisputeIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PutDueDateDisputeIdResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestPutDueDateDisputeId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutDueDateDisputeId \n" + exception + "\n");
            throw;
        }
    }
}