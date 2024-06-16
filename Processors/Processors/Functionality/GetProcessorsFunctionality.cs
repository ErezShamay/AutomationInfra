using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.Processors.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.Processors.Functionality;

public class GetProcessorsFunctionality
{
    private const string EndPoint = "/api/v1/processors";
    private readonly HttpSender _httpSender = new();
    
    public async Task<GetProcessorsResponse.Root> SendGetRequestGetProcessorsAsync(Dictionary<string, string> envDict,
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetProcessors");
            var response = await _httpSender.SendGetHttpsRequestAsync(envDict["EnvironmentVariables.processors"] + EndPoint + ipn, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetProcessorsResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetProcessors\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetProcessors\n" + exception + "\n");
            throw;
        }
    }
}