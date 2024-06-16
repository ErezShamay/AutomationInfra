using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsBaseObjects;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsApiFunctionality;

public class PostLayoutsIdFunctionality
{
    private const string EndPoint = "/api/v1/Layouts/";
    private readonly HttpSender _httpSender = new();
    private readonly PostLayoutsIdBaseObjects _postLayoutsIdBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostLayoutsIdResponse.Root> SendPostRequestPostLayoutsId(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostLayoutsId");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.Ams + EndPoint, 
                _postLayoutsIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostLayoutsIdResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostLayoutsId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostLayoutsId\n" + exception + "\n");
            throw;
        }
    }
}