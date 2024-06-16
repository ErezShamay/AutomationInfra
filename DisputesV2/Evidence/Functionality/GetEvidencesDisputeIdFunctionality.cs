using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Evidence.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Evidence.Functionality;

public class GetEvidencesDisputeIdFunctionality
{
    private const string EndPoint = "/api/v1/evidences/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetEvidencesDisputeIdResponse.Root> SendGetRequestGetEvidencesDisputeIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetEvidencesDisputeId");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.DisputesV2Url + EndPoint + disputeId, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetEvidencesDisputeIdResponse.Root>(response);
            if (jResponse!.Evidences.Count == 0)
            {
                for (var i = 0; i < 10; i++)
                {
                    await Task.Delay(5 * 1000);
                    response = await _httpSender.SendGetHttpsRequestAsync(
                        _envConfig.DisputesV2Url + EndPoint + disputeId, requestHeader);
                    jResponse = JsonConvert.DeserializeObject<GetEvidencesDisputeIdResponse.Root>(response);
                    if (jResponse!.Evidences.Count > 0)
                    {
                        return jResponse;
                    }
                }
            }
            Console.WriteLine("Done with SendGetRequestGetEvidencesDisputeId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetEvidencesDisputeId \n" + exception + "\n");
            throw;
        }
    }
}