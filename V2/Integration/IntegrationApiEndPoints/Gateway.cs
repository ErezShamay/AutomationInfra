using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V2.Integration.IntegrationBaseObjects;
using Splitit.Automation.NG.Backend.Services.V2.Integration.IntegrationResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V2.Integration.IntegrationApiEndPoints;

public class Gateway
{
    private const string GatewayEndPoint = "/api/integration/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<GatewayResponse.Root> SendGatewayRequestAsync(RequestHeader requestHeader, string gateway,
        GatewayBaseObject gatewayBaseObject)
    {
        try
        {
            Console.WriteLine("\nStarting SendGatewayRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.EmbededdInitiate + GatewayEndPoint + gateway,
                gatewayBaseObject, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GatewayResponse.Root>(response);
            Console.WriteLine("Done with SendGatewayRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGatewayRequest \n" + exception + "\n");
            throw;
        }
    }
}