using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsApiEndPoint;

public class NotifyReportReady
{
    private readonly string _notifyReportReadyEndPoint = "";
    private readonly HttpSender _httpSender = new();
    private readonly GenerateFileBaseObjects _generateFileBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<NotifyReportReadyResponse.Root> SendPostRequestForNotifyReportReady(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForNotifyReportReady");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + _notifyReportReadyEndPoint,
                _generateFileBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<NotifyReportReadyResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForNotifyReportReady\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForNotifyReportReady\n" + exception);
            throw;
        }
    }
}