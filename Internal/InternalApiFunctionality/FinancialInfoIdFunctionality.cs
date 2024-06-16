using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.Internal.InternalResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.Internal.InternalApiFunctionality;

public class FinancialInfoIdFunctionality
{
    private const string FinancialInfoIdEndPoint = "/api/v1/financial-info";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<FinancialInfoIdResponse.Root> SendGetRequestForFinancialInfoId( RequestHeader requestHeader, string id)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestForFinancialInfoId");
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.Ams + FinancialInfoIdEndPoint + "/" + id, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<FinancialInfoIdResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestForFinancialInfoId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestForFinancialInfoId \n" + exception + "\n");
            throw;
        }
    }
}