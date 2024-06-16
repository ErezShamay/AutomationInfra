using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Infrastructure.InfrastructureResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Infrastructure.InfrastructureApiEndPoint;

public class ValuesEnums
{
    private const string ValuesEnumsEndPoint = "/api/v1/infrastructure/values/enums";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ValuesEnumsResponse.Root> SendGetRequestForValuesEnumsAsync(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForValuesEnums");
            var path = _envConfig.ReportingSystemUrl + ValuesEnumsEndPoint;
            var response = await _httpSender.SendGetHttpsRequestAsync(path, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ValuesEnumsResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForValuesEnums\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForValuesEnums \n" + exception + "\n");
            throw;
        }
    }
}