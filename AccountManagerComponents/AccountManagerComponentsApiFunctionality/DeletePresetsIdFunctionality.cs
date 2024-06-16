using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsApiFunctionality;

public class DeletePresetsIdFunctionality
{
    private const string EndPoint = "/api/v1/Presets/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<DeletePresetsIdResponse.Root> SendDeletePresetsId(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeletePresetsId");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.Ams + EndPoint, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DeletePresetsIdResponse.Root>(response);
            Console.WriteLine("Done with SendDeletePresetsId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeletePresetsId \n" + exception + "\n");
            throw;
        }
    }
}