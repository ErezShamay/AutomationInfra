using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsApiEndPoint;

public class DownloadReportFile
{
    private const string DownloadReportFileEndPoint = "";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<DownloadReportFileResponse.Root> SendGetRequestForDownloadReportFileAsync(
        RequestHeader requestHeader, bool isByToken, string strToLookFor)
    {
        try
        {
            var queryAttribute = isByToken ? "?token=" : "?fileName=";
            Console.WriteLine("\nStarting SendGetRequestForDownloadReportFile");
            var path = _envConfig.ReportingSystemUrl + DownloadReportFileEndPoint + queryAttribute + strToLookFor;
            var response = await _httpSender.SendGetHttpsRequestAsync(path, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DownloadReportFileResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForDownloadReportFile\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForDownloadReportFile \n" + exception + "\n");
            throw;
        }
    }
}