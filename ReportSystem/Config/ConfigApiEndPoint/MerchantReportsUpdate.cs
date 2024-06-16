using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigApiEndPoint;

public class MerchantReportsUpdate
{
    private readonly string _merchantReportsUpdateEndPoint = "/api/v1/configuration/merchantreports/update";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly MerchantReportsUpdateBaseObjects _merchantReportsUpdateBaseObjects = new();

    public async Task<MerchantReportsUpdateResponse.Root> SendPostRequestForMerchantReportsUpdate(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForMerchantReportsUpdate");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + _merchantReportsUpdateEndPoint,
                _merchantReportsUpdateBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<MerchantReportsUpdateResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForMerchantReportsUpdate\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForMerchantReportsUpdate\n" + exception + "\n");
            throw;
        }
    }
}