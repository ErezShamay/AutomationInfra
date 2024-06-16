using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;

public class GetGenerateAsymmetricFunctionality
{
    private const string EndPoint = "/api/v1/sample/generate-asymmetric";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetGenerateAsymmetricResponse.Root> SendGetRequestGetGenerateAsymmetricAsync(
        RequestHeader requestHeader, string keyType, string length)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetGenerateAsymmetric");
            var stringBody = "{\r\n\"KeyType\": \""+keyType+"\",\r\n\"Length\": "+length+"\r\n}";
            var response = await _httpSender.SendGetWithBodyHttpsRequestAsync(_envConfig.KeyExchangeServerUrl + EndPoint,
                requestHeader, stringBody);
            var jResponse = JsonConvert.DeserializeObject<GetGenerateAsymmetricResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetGenerateAsymmetric\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetGenerateAsymmetric \n" + exception + "\n");
            throw;
        }
    }
}