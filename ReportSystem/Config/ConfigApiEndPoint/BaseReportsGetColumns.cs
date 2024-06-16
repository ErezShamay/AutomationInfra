using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigApiEndPoint;

public class BaseReportsGetColumns
{
    private const string BaseReportsGetColumnsEndPoint = "/api/v1/configuration/basereports/getcolumns";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<BaseReportsGetColumnsResponse.Root> SendGetRequestForBaseReportsGetColumnsAsync( RequestHeader requestHeader, string dataSourceId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForBaseReportsGetColumns");
            var path = _envConfig.ReportingSystemUrl + BaseReportsGetColumnsEndPoint + "?dataSourceId=" + dataSourceId;
            var response = await _httpSender.SendGetHttpsRequestAsync(path, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<BaseReportsGetColumnsResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForBaseReportsGetColumns\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForBaseReportsGetColumns \n" + exception + "\n");
            throw;
        }
    }
}