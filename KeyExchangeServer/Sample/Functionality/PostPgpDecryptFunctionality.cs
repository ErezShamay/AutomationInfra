using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.BaseObjects;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;

public class PostPgpDecryptFunctionality
{
    private const string EndPoint = "/api/v1/sample/pgp/decrypt";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendPostRequestPostPgpDecryptAsync(
        RequestHeader requestHeader, PostPgpDecryptBaseObjects.Root postPgpDecrypt)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostPgpDecryptAsync");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint, 
                postPgpDecrypt, requestHeader);
            Console.WriteLine("Done with SendPostRequestPostPgpDecryptAsync\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostPgpDecryptAsync\n" + exception + "\n");
            throw;
        }
    }
}