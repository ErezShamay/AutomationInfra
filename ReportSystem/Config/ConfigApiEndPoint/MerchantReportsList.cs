using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigApiEndPoint;

public class MerchantReportsList
{
    private const string MerchantReportsListEndPoint = "/api/v1/configuration/merchantreports/list";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<MerchantReportsListResponse.Root> SendGetRequestForMerchantReportsListAsync(
        RequestHeader requestHeader, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForMerchantReportsList");
            var path = _envConfig.ReportingSystemUrl + MerchantReportsListEndPoint + "?MerchantId=" + merchantId;
            var response = await _httpSender.SendGetHttpsRequestAsync(path, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<MerchantReportsListResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForMerchantReportsList\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForMerchantReportsList \n" + exception + "\n");
            throw;
        }
    }
}