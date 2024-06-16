using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PutLiabilityDisputeIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/liability/";
    private readonly HttpSender _httpSender = new();
    private readonly PutLiabilityDisputeIdBaseObjects _putLiabilityDisputeIdBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<PutLiabilityDisputeIdResponse.Root> SendPutRequestPutLiabilityDisputeIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutLiabilityDisputeId");
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint + disputeId,
                _putLiabilityDisputeIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PutLiabilityDisputeIdResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestPutLiabilityDisputeId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutLiabilityDisputeId \n" + exception + "\n");
            throw;
        }
    }
}