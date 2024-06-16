using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.ChargeBack.Functionality;

public class DeleteDisputesIdDeleteCommentCommentIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendDeleteRequestDeleteDisputesIdDeleteCommentCommentIdAsync(
        RequestHeader requestHeader, string disputeId, string commentId, int merchantId)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeleteRequestDeleteDisputesIdDeleteCommentCommentId");
            var response = await _httpSender.SendDeleteHttpsRequestAsync(
                _envConfig.MerchantApiUrl + EndPoint + disputeId + "/delete-comment/" + commentId, 
                requestHeader, "yes", merchantId.ToString());
            if (!response.Equals(""))
            {
                for (var i = 0; i < 10; i++)
                {
                    await Task.Delay(5 * 1000);
                    response = await _httpSender.SendDeleteHttpsRequestAsync(
                        _envConfig.MerchantApiUrl + EndPoint + disputeId + "/delete-comment/" + commentId, 
                        requestHeader, "yes", merchantId.ToString());
                    if (response.Equals(""))
                    {
                        return response;
                    }
                }
            }
            Console.WriteLine("Done with SendDeleteRequestDeleteDisputesIdDeleteCommentCommentId\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeleteRequestDeleteDisputesIdDeleteCommentCommentId \n" + exception + "\n");
            throw;
        }
    }
}