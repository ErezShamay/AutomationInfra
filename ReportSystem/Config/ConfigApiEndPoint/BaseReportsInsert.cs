using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigApiEndPoint;

public class BaseReportsInsert
{
    private const string BaseReportsInsertEndPoint = "/api/v1/configuration/basereports/insert";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly BaseReportsInsertBaseObjects _baseReportsInsertBaseObjects = new();

    public async Task<BaseReportsInsertResponse.Root> SendPostRequestForBaseReportsInsert(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForBaseReportsInsert");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + BaseReportsInsertEndPoint,
                _baseReportsInsertBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<BaseReportsInsertResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForBaseReportsInsert\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForBaseReportsInsert\n" + exception + "\n");
            throw;
        }
    }
}