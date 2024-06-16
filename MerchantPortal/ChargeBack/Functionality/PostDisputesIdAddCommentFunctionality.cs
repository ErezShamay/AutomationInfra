using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;

public class PostDisputesIdAddCommentFunctionality
{
    private const string EndPoint = "/api/v1/disputes/";
    private readonly HttpSender _httpSender = new();
    private readonly PostDisputesIdAddCommentBaseObjects _posDisputesBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostDisputesIdAddCommentResponse.Root> SendPostRequestPostDisputesIdAddCommentAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostDisputes");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint + disputeId + "/add-comment",
                _posDisputesBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostDisputesIdAddCommentResponse.Root>(response);
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