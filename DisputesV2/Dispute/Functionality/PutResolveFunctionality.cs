using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PutResolveFunctionality
{
    private const string EndPoint = "/api/v1/disputes/resolve";
    private readonly HttpSender _httpSender = new();
    private readonly PutResolveBaseObjects _putResolveBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<string> SendPutRequestPutResolveAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutResolve");
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint,
                _putResolveBaseObjects, requestHeader);
            //var jResponse = JsonConvert.DeserializeObject<PutLiabilityDisputeIdResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestPutResolve\n");
            return response!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutResolve \n" + exception + "\n");
            throw;
        }
    }
}