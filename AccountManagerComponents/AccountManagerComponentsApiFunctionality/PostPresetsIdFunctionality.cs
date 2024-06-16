using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsBaseObjects;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsApiFunctionality;

public class PostPresetsIdFunctionality
{
    private const string EndPoint = "/api/v1/Presets/";
    private readonly HttpSender _httpSender = new();
    private readonly PostPresetsIdBaseObjects _postPresetsIdBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostPresetsIdResponse.Root> SendPostRequestPostPresetsId(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostPresetsId");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.Ams + EndPoint, 
                _postPresetsIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostPresetsIdResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostPresetsId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostPresetsId\n" + exception + "\n");
            throw;
        }
    }
}