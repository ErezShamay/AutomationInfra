using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.AuthenticationParameters.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.AuthenticationParameters.Functionality;

public class GetProcessorProcessorIdFunctionality
{
    private const string EndPoint = "/api/v1/authentication-parameters/processor/";
    private readonly HttpSender _httpSender = new();
    
    public async Task<GetProcessorProcessorIdResponse.Root> SendGetRequestGetProcessorProcessorIdAsync(Dictionary<string, string> envDict,
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetProcessorProcessorId");
            var response = await _httpSender.SendGetHttpsRequestAsync(envDict["EnvironmentVariables.processors"] + EndPoint + ipn, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetProcessorProcessorIdResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetProcessorProcessorId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetProcessorProcessorId\n" + exception + "\n");
            throw;
        }
    }
}