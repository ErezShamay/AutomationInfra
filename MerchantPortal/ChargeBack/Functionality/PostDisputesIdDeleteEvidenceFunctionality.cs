using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;

public class PostDisputesIdDeleteEvidenceFunctionality
{
    private const string EndPoint = "/api/v1/disputes/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PostDisputesIdDeleteEvidenceBaseObjects.Root _postDisputesIdDeleteEvidenceBaseObjects = new();

    public async Task<PostDisputesIdDeleteEvidenceResponse.Root> SendPostRequestPostDisputesIdDeleteEvidenceAsync(
        RequestHeader requestHeader, string disputeId, string evidenceId, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostDisputesIdDeleteEvidence");
            _postDisputesIdDeleteEvidenceBaseObjects.Ids = new List<string> { evidenceId };
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint + disputeId + "/delete-evidence",
                _postDisputesIdDeleteEvidenceBaseObjects, requestHeader, null!, null!, "yes", merchantId);
            var jResponse = JsonConvert.DeserializeObject<PostDisputesIdDeleteEvidenceResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostDisputesIdDeleteEvidence\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostDisputesIdDeleteEvidence\n" + exception + "\n");
            throw;
        }
    }
}