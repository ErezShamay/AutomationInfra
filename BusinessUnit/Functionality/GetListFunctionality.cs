using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.BusinessUnit.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.BusinessUnit.Functionality;

public class GetListFunctionality
{
    private const string EndPoint = "/api/business-unit/list";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetListResponse.Root> SendGetRequestGetListAsync(
        RequestHeader requestHeader, string merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetListAsync");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.AdminUrl + EndPoint + "?MerchantId=" + merchantId, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetListResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetListAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetListAsync \n" + exception + "\n");
            throw;
        }
    }
}