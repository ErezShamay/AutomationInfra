using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.BaseObjects;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;

public class PostAcceptDisputesIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/accept/";
    private readonly HttpSender _httpSender = new();
    private readonly PostAcceptDisputesIdBaseObjects.Root _postAcceptDisputesIdBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostAcceptDisputesIdResponse.Root> SendPostRequestPostAcceptDisputesIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostAcceptDisputesId");
            _postAcceptDisputesIdBaseObjects.Accept = true;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint + disputeId,
                _postAcceptDisputesIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostAcceptDisputesIdResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostAcceptDisputesId\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostAcceptDisputesId \n" + exception + "\n");
            throw;
        }
    }
}