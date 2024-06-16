using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Disputes.DisputesBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.Disputes.DisputesResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Disputes.DisputesFunctionality;

public class SetDisputeLiabilityId
{
    private const string SetDisputeLiabilityIdEndPoint = "/api/disputeCharges/set-dispute-liability/";
    private readonly HttpSender _httpSender = new();
    private readonly SetDisputeLiabilityBaseObjects.Root _setDisputeLiabilityBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<SetDisputeLiabilityIdResponse.Root?> SendPutRequestSetDisputeLiabilityIdAsync(RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestSetDisputeLiabilityId");
            _setDisputeLiabilityBaseObjects.DisputeLiability = "Merchant";
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.AdminUrl + SetDisputeLiabilityIdEndPoint + disputeId,
                _setDisputeLiabilityBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<SetDisputeLiabilityIdResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestSetDisputeLiabilityId\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestSetDisputeLiabilityId\n" + exception + "\n");
            throw;
        }
    }
}