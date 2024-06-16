using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PutAcceptDisputeIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/accept/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PutAcceptDisputeIdBaseObjects _putAcceptDisputeIdBaseObjects = new();
    
    public async Task<PutAcceptDisputeIdResponse.Root> SendPutRequestPutAcceptDisputeIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutAcceptDisputeId");
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint + disputeId,
                _putAcceptDisputeIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PutAcceptDisputeIdResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestPutAcceptDisputeId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutAcceptDisputeId \n" + exception + "\n");
            throw;
        }
    }
}