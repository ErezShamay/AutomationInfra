using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigApiEndPoint;

public class BaseReportsList
{
    private const string BaseReportsListEndPoint = "/api/v1/configuration/basereports/list";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<BaseReportsListResponse.Root> SendGetRequestForBaseReportListAsync(
        RequestHeader requestHeader, string reportId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForBaseReportList");
            var path = _envConfig.ReportingSystemUrl + BaseReportsListEndPoint + "?ReportId=" + reportId;
            var response = await _httpSender.SendGetHttpsRequestAsync(path, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<BaseReportsListResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForBaseReportList\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForBaseReportList \n" + exception + "\n");
            throw;
        }
    }
}