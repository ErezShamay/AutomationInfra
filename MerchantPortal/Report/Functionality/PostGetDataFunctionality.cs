using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Functionality;

public class PostGetDataFunctionality
{
    private const string EndPoint = "/api/v1/report/get-data";
    private readonly HttpSender _httpSender = new();
    private readonly PostGetDataBaseObjects _postGetDataBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostGetDataResponse.Root> SendPostRequestPostGetDataAsync(
        RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostGetData");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint,
                _postGetDataBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostGetDataResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostGetData\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostGetData\n" + exception + "\n");
            throw;
        }
    }
}