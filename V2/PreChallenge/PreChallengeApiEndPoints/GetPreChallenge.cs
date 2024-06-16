using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V2.PreChallenge.PreChallengeResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V2.PreChallenge.PreChallengeApiEndPoints;

public class GetPreChallenge
{
    private const string GetPreChallengeEndPoint = "/api/PreChallenge";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetPreChallengeResponse.Root?> SendGetPreChallengeRequestAsync( RequestHeader requestHeader, int planNumber)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetPreChallengeRequest");
            var endPoint = _envConfig.EmbededdInitiate + GetPreChallengeEndPoint + "?planNumber=" + planNumber;
            var response = await _httpSender.SendGetHttpsRequestAsync(endPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetPreChallengeResponse.Root>(response);
            Console.WriteLine("Done SendGetPreChallengeRequest\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SendGetPreChallengeRequest \n" + exception + "\n");
            throw;
        }
    }
}