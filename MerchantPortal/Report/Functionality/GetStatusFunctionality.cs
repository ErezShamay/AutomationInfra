using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Functionality;

public class GetStatusFunctionality
{
    private const string EndPoint = "/api/v1/report/plan/aggregation/status";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetStatusResponse.Root> SendGetRequestGetStatusAsync(
        RequestHeader requestHeader, string fromUtc = default!, string toUtc = default!)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetGetStatus");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetStatusResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetStatus\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetStatus\n" + exception + "\n");
            throw;
        }
    }
}