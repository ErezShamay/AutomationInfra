using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsApiEndPoint;

public class GenerateReport
{
    private const string EndPoint = "/api/v1/reports/generate-report";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly GenerateReportBaseObjects.Root _generateReportBaseObjects = new();

    public async Task<GenerateReportResponse.Root> SendPostRequestForGenerateReportAsync(
        RequestHeader requestHeader, string reportCode, DateTime from, DateTime to, string format, List<string> selectedColumns)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForGenerateReportAsync");
            _generateReportBaseObjects.ReportCode = reportCode;
            _generateReportBaseObjects.From = from;
            _generateReportBaseObjects.To = to;
            _generateReportBaseObjects.Format = format;
            _generateReportBaseObjects.SelectedColumns = selectedColumns;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + EndPoint,
                _generateReportBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GenerateReportResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForGenerateReportAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForGenerateFile\n" + exception);
            throw;
        }
    }
}