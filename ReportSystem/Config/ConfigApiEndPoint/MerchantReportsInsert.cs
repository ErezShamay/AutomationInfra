using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigBaseObjects;
using Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.ReportSystem.Config.ConfigApiEndPoint;

public class MerchantReportsInsert
{
    private const string MerchantReportsInsertEndPoint = "/api/v1/configuration/merchantreports/insert";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly MerchantReportsInsertBaseObjects _merchantReportsInsertBaseObjects = new();

    public async Task<BaseReportsUpdateResponse.Root> SendPostRequestForMerchantReportsInsertAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestForMerchantReportsInsert");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.ReportingSystemUrl + MerchantReportsInsertEndPoint,
                _merchantReportsInsertBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<BaseReportsUpdateResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestForMerchantReportsInsert\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestForMerchantReportsInsert\n" + exception + "\n");
            throw;
        }
    }
}