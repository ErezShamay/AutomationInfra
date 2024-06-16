using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.AccountManagerComponents.AccountManagerComponentsApiFunctionality;

public class DeleteLayoutsIdFunctionality
{
    private const string EndPoint = "/api/v1/Layouts/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<DeleteLayoutsIdResponse.Root> SendDeleteLayoutsId(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeleteLayoutsId");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.Ams + EndPoint, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DeleteLayoutsIdResponse.Root>(response);
            Console.WriteLine("Done with SendDeleteLayoutsId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeleteLayoutsId \n" + exception + "\n");
            throw;
        }
    }
}