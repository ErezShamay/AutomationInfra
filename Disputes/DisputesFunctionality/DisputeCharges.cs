using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.Disputes.DisputesResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.Disputes.DisputesFunctionality;

public class DisputeCharges
{
    private const string DisputeChargesEndPoint = "/api/disputeCharges";
    private readonly HttpSender _httpSender = new();
    private int _counter;
    private readonly EnvConfig _envConfig = new();

    public async Task<DisputeChargesResponse.Root> SendGetDisputeChargesAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetDisputeCharges");
            var endPoint = _envConfig.AdminUrl + DisputeChargesEndPoint + "?InstallmentPlanNumber=" + ipn;
            var response = await _httpSender.SendGetHttpsRequestAsync(endPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DisputeChargesResponse.Root>(response);
            if (jResponse!.Disputes.Count == 0 && _counter < 3)
            {
                jResponse = await SendGetDisputeChargesAsync(requestHeader, ipn);
                _counter++;
            }
            Console.WriteLine("Done SendGetDisputeCharges\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SendGetDisputeCharges \n" + exception + "\n");
            throw;
        }
    }
}