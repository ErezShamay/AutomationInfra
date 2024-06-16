using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Reports.ReportsApiEndPoint;

public class GetSample
{
    private const string GetSampleEndPoint = "";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly GetSampleBaseObjects _getSampleBaseObjects = new();

    public async Task<GetSampleResponse.Root> SendPostRequestForGetSampleAsync(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForGetSample");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + GetSampleEndPoint,
                _getSampleBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetSampleResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForGetSample\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForGetSample\n" + exception + "\n");
            throw;
        }
    }
}