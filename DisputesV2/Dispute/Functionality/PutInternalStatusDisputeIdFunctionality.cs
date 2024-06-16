using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PutInternalStatusDisputeIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/internal-status/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PutInternalStatusDisputeIdBaseObjects.Root _putInternalStatusDisputeIdBaseObjects = new();
    
    public async Task<PutInternalStatusDisputeIdResponse.Root> SendPutRequestPutInternalStatusDisputeIdAsync(
        RequestHeader requestHeader, string disputeId, string disputeProgress)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutInternalStatusDisputeId");
            _putInternalStatusDisputeIdBaseObjects.DisputeProgress = disputeProgress;
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint + disputeId,
                _putInternalStatusDisputeIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PutInternalStatusDisputeIdResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestPutInternalStatusDisputeId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutInternalStatusDisputeId \n" + exception + "\n");
            throw;
        }
    }
}