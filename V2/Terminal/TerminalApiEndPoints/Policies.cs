using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V2.Terminal.TerminalResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V2.Terminal.TerminalApiEndPoints;

public class Policies
{
    private const string PoliciesEndPoint = "/api/terminal/policies";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<PoliciesResponse.Root?> SendGetGetRefRequestAsync(RequestHeader requestHeader, int terminalId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetGetRefRequest");
            var endPoint = _envConfig.EmbededdInitiate + PoliciesEndPoint + "?TerminalQueryCriteria.TerminalId=" + terminalId;
            var response = await _httpSender.SendGetHttpsRequestAsync(endPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PoliciesResponse.Root>(response);
            Console.WriteLine("Done SendGetGetRefRequest\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SendGetGetRefRequest \n" + exception + "\n");
            throw;
        }
    }
}