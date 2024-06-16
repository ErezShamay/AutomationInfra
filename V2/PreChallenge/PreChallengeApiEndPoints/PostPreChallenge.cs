using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V2.PreChallenge.PreChallengeBaseObjects;
using Splitit.Automation.NG.Backend.Services.V2.PreChallenge.PreChallengeResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V2.PreChallenge.PreChallengeApiEndPoints;

public class PostPreChallenge
{
    private const string PostPreChallengeEndPoint = "/api/PreChallenge";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostPreChallengeResponse.Root> SendPostPreChallengeRequestAsync( RequestHeader requestHeader, string planNumber,
        PreChallengeBaseObject preChallengeBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostPreChallengeRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.EmbededdInitiate + PostPreChallengeEndPoint + planNumber,
                preChallengeBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostPreChallengeResponse.Root>(response);
            Console.WriteLine("Done with SendPostPreChallengeRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostPreChallengeRequest \n" + exception + "\n");
            throw;
        }
    }
}