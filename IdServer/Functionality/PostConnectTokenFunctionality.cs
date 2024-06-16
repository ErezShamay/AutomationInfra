using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.IdServer.requests;
using Splitit.Automation.NG.Backend.Services.IdServer.responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.IdServer.Functionality;

public class PostConnectTokenFunctionality
{
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostConnectTokenResponse.Root?> SendPostRequestPostConnectTokenAsync(
        RequestHeader requestHeader, PostConnectTokenBaseObjects.Root postPgpDecrypt)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostConnectTokenAsync");
            var bodyReq = "grant_type="+postPgpDecrypt.grant_type+"&scope=" + postPgpDecrypt.scope + 
                          "&client_id="+postPgpDecrypt.client_id+"&client_secret=" + postPgpDecrypt.client_secret;
            var response =  await _httpSender.SendPostHttpRequestStringBody(_envConfig.ShopperPortalLoginUrl, 
                bodyReq);
            var jResponse = JsonConvert.DeserializeObject<PostConnectTokenResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostConnectTokenAsync\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostConnectTokenAsync\n" + exception + "\n");
            throw;
        }
    }
}