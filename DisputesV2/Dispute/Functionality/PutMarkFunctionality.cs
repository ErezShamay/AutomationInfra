using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PutMarkFunctionality
{
    private const string EndPoint = "/api/v1/disputes/liability/";
    private readonly HttpSender _httpSender = new();
    private readonly PutMarkBaseObjects _putMarkBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<string> SendPutRequestPutMarkAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutMark");
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint,
                _putMarkBaseObjects, requestHeader);
            //var jResponse = JsonConvert.DeserializeObject<PutLiabilityDisputeIdResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestPutMark\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutMark \n" + exception + "\n");
            throw;
        }
    }
}