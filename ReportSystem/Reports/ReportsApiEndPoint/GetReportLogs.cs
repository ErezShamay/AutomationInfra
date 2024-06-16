using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsApiEndPoint;

public class GetReportLogs
{
    private const string GetReportLogsEndPoint = "";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly GetReportLogsBaseObjects _getReportLogsBaseObjects = new();

    public async Task<GetReportLogsResponse.Root> SendPostRequestForGetReportLogsAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForGetReportLogs");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + GetReportLogsEndPoint,
                _getReportLogsBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetReportLogsResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForGetReportLogs\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForGetReportLogs\n" + exception + "\n");
            throw;
        }
    }
}