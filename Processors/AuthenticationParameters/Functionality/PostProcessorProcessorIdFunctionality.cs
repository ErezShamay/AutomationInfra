using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.AuthenticationParameters.BaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.AuthenticationParameters.Functionality;

public class PostProcessorProcessorIdFunctionality
{
    private const string EndPoint = "/api/v1/authentication-parameters/processor/";
    private readonly HttpSender _httpSender = new();
    private readonly PostProcessorProcessorIdBaseObjects _postProcessorProcessorIdBaseObjects = new();

    public async Task<string> SendPostRequestPostProcessorProcessorIdAsync(Dictionary<string, string> envDict, RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostProcessorProcessorId");
            var response = await _httpSender.SendPostHttpsRequestAsync(envDict["EnvironmentVariables.Processors"] + EndPoint,
                _postProcessorProcessorIdBaseObjects, requestHeader);
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