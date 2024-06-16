using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsApiFunctionality;

public class DeletePagesIdFunctionality
{
    private const string EndPoint = "/api/v1/Pages/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<DeletePagesIdResponse.Root> SendDeletePagesId(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeletePagesId");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.Ams + EndPoint, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DeletePagesIdResponse.Root>(response);
            Console.WriteLine("Done with SendDeletePagesId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeletePagesId \n" + exception + "\n");
            throw;
        }
    }
}