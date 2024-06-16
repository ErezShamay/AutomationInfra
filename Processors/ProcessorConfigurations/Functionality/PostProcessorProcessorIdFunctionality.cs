using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.ProcessorConfigurations.BaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.ProcessorConfigurations.Functionality;

public class PostProcessorProcessorIdFunctionality
{
    private const string EndPoint = "/api/v1/processors-configurations/processor/";
    private readonly HttpSender _httpSender = new();

    public async Task<string> SendPostRequestPostProcessorProcessorIdAsync(Dictionary<string, string> envDict, RequestHeader requestHeader,
        PostProcessorProcessorIdBaseObjects postProcessorProcessorIdBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostProcessorProcessorId");
            var response = await _httpSender.SendPostHttpsRequestAsync(envDict["EnvironmentVariables.Processors"] + EndPoint,
                postProcessorProcessorIdBaseObjects, requestHeader);
            //var jResponse = JsonConvert.DeserializeObject<.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostProcessorProcessorId\n");
            return response!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostProcessorProcessorId \n" + exception + "\n");
            throw;
        }
    }
}