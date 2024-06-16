using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Evidence.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Evidence.Functionality;

public class DeleteEvidencesFunctionality
{
    private const string EndPoint = "/api/v1/evidences";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<DeleteEvidencesResponse.Root> SendDeleteRequestDeleteEvidencesAsync(
        RequestHeader requestHeader, string disputeId, string evidenceId)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeleteRequestDeleteEvidences");
            var response = await _httpSender.SendDeleteHttpsRequestAsync(
                _envConfig.DisputesV2Url + EndPoint + "?DisputeId=" + disputeId + "&EvidenceIds=" + evidenceId, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DeleteEvidencesResponse.Root>(response);
            Console.WriteLine("Done with SendDeleteRequestDeleteEvidences\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeleteRequestDeleteEvidences \n" + exception + "\n");
            throw;
        }
    }
}