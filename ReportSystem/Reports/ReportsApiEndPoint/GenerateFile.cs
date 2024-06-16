using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsApiEndPoint;

public class GenerateFile
{
    private const string GenerateFileEndPoint = "/api/v1/reports/generate-file";
    private readonly HttpSender _httpSender = new();
    private readonly GenerateFileBaseObjects.Root _generateFileBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<GenerateFileResponse.Root> SendPostRequestForGenerateFileAsync(
        RequestHeader requestHeader, string reportId, 
        DateTime from, DateTime to, int format, int merchantId, List<string> selectedColumns
        , int businessUnit)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForGenerateFile");
            _generateFileBaseObjects.ReportId = reportId;
            _generateFileBaseObjects.From = from;
            _generateFileBaseObjects.To = to;
            _generateFileBaseObjects.Format = format;
            _generateFileBaseObjects.TimeZoneId = "UTC";
            _generateFileBaseObjects.BusinessUnits = new List<int> { businessUnit };
            _generateFileBaseObjects.MerchantId = merchantId;
            _generateFileBaseObjects.SelectedColumns = new List<string>();
            _generateFileBaseObjects.SelectedColumns = selectedColumns;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + GenerateFileEndPoint,
                _generateFileBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GenerateFileResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForGenerateFile\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForGenerateFile\n" + exception);
            throw;
        }
    }
}