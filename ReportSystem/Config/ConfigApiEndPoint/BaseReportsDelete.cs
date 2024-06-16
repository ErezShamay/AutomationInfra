using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigApiEndPoint;

public class BaseReportsDelete
{
    private const string BaseReportsDeleteEndPoint = "/api/v1/configuration/basereports/delete";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<BaseReportsDeleteResponse.Root> SendGetRequestForBaseReportsDeleteAsync( RequestHeader requestHeader, string reportId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForBaseReportsDelete");
            var path = _envConfig.ReportingSystemUrl + BaseReportsDeleteEndPoint + "?ReportId=" + reportId;
            var response = await _httpSender.SendDeleteHttpsRequestAsync(path, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<BaseReportsDeleteResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForBaseReportsDelete\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForBaseReportsDelete \n" + exception + "\n");
            throw;
        }
    }
}