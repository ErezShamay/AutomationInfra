using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.Processors.BaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.Processors.Functionality;

public class PutDeleteFunctionality
{
    private const string CreatedEndPoint= "/api/v1/processors/delete";
    private readonly PutDeleteBaseObjects _putDeleteBaseObjects = new();
    private readonly HttpSender _httpSender = new();

    public async Task<string> SendPutRequestPutDeleteAsync(Dictionary<string, string> envDict, RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutDelete");
            var response = await _httpSender.SendPutHttpsRequestAsync(envDict["EnvironmentVariables.processors"] + CreatedEndPoint,
                _putDeleteBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPutRequestPutDelete\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutDelete\n" + exception + "\n");
            throw;
        }
    }
}