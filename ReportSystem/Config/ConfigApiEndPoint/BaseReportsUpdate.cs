using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigApiEndPoint;

public class BaseReportsUpdate
{
    private const string BaseReportsUpdateEndPoint = "/api/v1/configuration/basereports/update";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly BaseReportsUpdateBaseObjects _baseReportsUpdateBaseObjects = new();

    public async Task<BaseReportsUpdateResponse.Root> SendPostRequestForBaseReportsUpdateAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForBaseReportsUpdate");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + BaseReportsUpdateEndPoint,
                _baseReportsUpdateBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<BaseReportsUpdateResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForBaseReportsUpdate\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForBaseReportsUpdate\n" + exception + "\n");
            throw;
        }
    }
}