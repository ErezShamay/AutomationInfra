using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;

public class PostDisputesFunctionality
{
    private const string EndPoint = "/api/v1/disputes";
    private readonly HttpSender _httpSender = new();
    private readonly PostDisputesBaseObjects.Root _postDisputesBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostDisputesResponse.Root> SendPostRequestPostDisputesAsync(
        RequestHeader requestHeader, string ipn, int merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostDisputes");
            _postDisputesBaseObjects.InstallmentPlanNumbers = new List<string> { ipn };
            _postDisputesBaseObjects.MerchantId = merchantId;
            _postDisputesBaseObjects.PagingRequest = new PostDisputesBaseObjects.PagingRequest
            {
                Skip = 0,
                Take = 10
            };
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint,
                _postDisputesBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostDisputesResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostDisputes\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostDisputes \n" + exception + "\n");
            throw;
        }
    }
}