using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.ProcessorConfigurations.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.ProcessorConfigurations.Functionality;

public class GetConfigurationsAllFunctionality
{
    private const string EndPoint = "/api/v1/processors-configurations/processor/";
    private readonly HttpSender _httpSender = new();
    
    public async Task<GetConfigurationsAllResponse.Root> SendGetRequestGetConfigurationsAllResponseAsync(Dictionary<string, string> envDict,
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetConfigurationsAllResponse");
            var response = await _httpSender.SendGetHttpsRequestAsync(envDict["EnvironmentVariables.processors"] + EndPoint + ipn, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetConfigurationsAllResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetConfigurationsAllResponse\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetConfigurationsAllResponse\n" + exception + "\n");
            throw;
        }
    }
}