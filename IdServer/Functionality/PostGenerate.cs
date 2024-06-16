using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.IdServer.requests;
using Splitit.Automation.NG.Backend.Services.IdServer.responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.IdServer.Functionality;

public class PostGenerate
{
    private const string EndPoint = "/api/clients/credentials/generate";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PostGenerateBaseObjects.Root _postGenerateBaseObjects = new();
    
    public async Task<PostGenerateResponse.Root?> SendPostRequestPostGenerateAsync(RequestHeader requestHeader, string clientId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostGenerate");
            _postGenerateBaseObjects.ClientId = clientId;
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.IdentityServerUrlHttp + EndPoint,
                _postGenerateBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostGenerateResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostGenerate\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostGenerate\n" + exception + "\n");
            throw;
        }
    }
}