using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Processors.UnclassifiedGatewayErrors.BaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.Processors.UnclassifiedGatewayErrors.Functionality;

public class PutMoveFunctionality
{
    private const string CreatedEndPoint= "/api/v1/unclassified-gateway-errors/move";
    private readonly PutMoveBaseObjects _putMoveBaseObjects = new();
    private readonly HttpSender _httpSender = new();

    public async Task<string> SendPutRequestPutMove(Dictionary<string, string> envDict, RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutMove");
            var response = await _httpSender.SendPutHttpsRequestAsync(envDict["EnvironmentVariables.processors"] + CreatedEndPoint,
                _putMoveBaseObjects, requestHeader);
            Console.WriteLine("Done with SendPutRequestPutMove\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutMove\n" + exception + "\n");
            throw;
        }
    }
}