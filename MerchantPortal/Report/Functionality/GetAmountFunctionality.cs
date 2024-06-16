using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Functionality;

public class GetAmountFunctionality
{
    private const string EndPoint = "/api/v1/report/plan/aggregation/amount";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetAmountResponse.Root> SendGetRequestGetEvidencesDisputeIdAsync(
        RequestHeader requestHeader, string fromUtc = default!, string toUtc = default!)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetEvidencesDisputeId");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetAmountResponse.Root>(response);
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