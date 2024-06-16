using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Functionality;

public class GetCountryFunctionality
{
    private const string EndPoint = "/api/v1/report/plan/aggregation/country";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetCountryResponse.Root> SendGetRequestGetGetCountryAsync(
        RequestHeader requestHeader, string fromUtc = default!, string toUtc = default!)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetCountry");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetCountryResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetCountry\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetCountry\n" + exception + "\n");
            throw;
        }
    }
}