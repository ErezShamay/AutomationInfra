using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.ProcessorConfigurations.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.ProcessorConfigurations.Functionality;

public class GetImportFunctionality
{
    private const string EndPoint = "/api/v1/processors-configurations/import";
    private readonly HttpSender _httpSender = new();
    
    public async Task<GetImportResponse.Root> SendGetRequestGetImportAsync(Dictionary<string, string> envDict,
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequest");
            var response = await _httpSender.SendGetHttpsRequestAsync(envDict["EnvironmentVariables.processors"] + EndPoint + ipn, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetImportResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequest\n" + exception + "\n");
            throw;
        }
    }
}