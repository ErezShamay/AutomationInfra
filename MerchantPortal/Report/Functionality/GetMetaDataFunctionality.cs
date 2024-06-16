using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Functionality;

public class GetMetaDataFunctionality
{
    private const string EndPoint = "/api/v1/report/metadata";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetMetaDataResponse.Root> SendGetRequestGetMetaDataAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetMetaData");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetMetaDataResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetMetaData\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetMetaData\n" + exception + "\n");
            throw;
        }
    }
}