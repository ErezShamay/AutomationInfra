using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.Processors.BaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.Processors.Functionality;

public class PostProcessorsIdFunctionality
{
    private const string EndPoint = "/api/v1/processors/";
    private readonly HttpSender _httpSender = new();

    public async Task<string> SendPostRequestPostProcessorProcessorIdAsync(Dictionary<string, string> envDict, RequestHeader requestHeader,
        PostProcessorsIdBaseObjects postProcessorProcessorIdBaseObjects)
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
            Console.WriteLine("Error in SendPostRequestPostProcessorProcessorId\n" + exception + "\n");
            throw;
        }
    }
}