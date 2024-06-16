using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyBaseObjects;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyResponse;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Key.KeyFunctionality;

public class PutSetExpirationFunctionality
{
    private const string EndPoint= "/api/v1/key/";
    private readonly PutSetExpirationBaseObjects _putSetExpirationBaseObjects = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PutSetExpirationResponse.Root> SendPutRequestPutSetExpirationAsync(RequestHeader requestHeader, string id)
    {
        try
        {
            Console.WriteLine("\nStarting SendPutRequestPutSetExpiration");
            var response = await _httpSender.SendPutHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + 
                                                                 EndPoint + id + "/" + "set-expiration",
                _putSetExpirationBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PutSetExpirationResponse.Root>(response);
            Console.WriteLine("Done with SendPutRequestPutSetExpiration\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPutRequestPutSetExpiration\n" + exception + "\n");
            throw;
        }
    }
}