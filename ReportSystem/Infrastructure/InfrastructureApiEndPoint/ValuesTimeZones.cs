using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Infrastructure.InfrastructureResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Infrastructure.InfrastructureApiEndPoint;

public class ValuesTimeZones
{
    private const string ValuesTimeZonesEndPoint = "";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ValuesTimeZonesResponse.Root> SendGetRequestForValuesTimeZonesAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForValuesTimeZones");
            var path = _envConfig.ReportingSystemUrl + ValuesTimeZonesEndPoint;
            var response = await _httpSender.SendGetHttpsRequestAsync(path, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ValuesTimeZonesResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForValuesTimeZones\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForValuesTimeZones \n" + exception + "\n");
            throw;
        }
    }
}