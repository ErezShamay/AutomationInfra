using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsApiEndPoint;

public class DownloadFileFromLog
{
    private const string DownloadFileFromLogEndPoint = "/api/v1/reports/download-file-from-log";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<DownloadFileFromLogResponse.Root> SendGetRequestForDownloadFileFromLogAsync(RequestHeader requestHeader, string logId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForDownloadFileFromLog");
            var path = _envConfig.ReportingSystemUrl + DownloadFileFromLogEndPoint + "?logId=" + logId;
            var response = await _httpSender.SendGetHttpsRequestAsync(path, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DownloadFileFromLogResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForDownloadFileFromLog\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForDownloadFileFromLog \n" + exception + "\n");
            throw;
        }
    }
}