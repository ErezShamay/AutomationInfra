using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PutStatusFunctionality
{
    private const string EndPoint = "/api/v1/disputes/status/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PutStatusBaseObjects _putStatusBaseObjects = new();
    
    public async Task<PutStatusResponse.Root> SendPutRequestPutStatusAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutStatus");
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint + disputeId,
                _putStatusBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PutStatusResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestPutStatus\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutStatus \n" + exception + "\n");
            throw;
        }
    }
}