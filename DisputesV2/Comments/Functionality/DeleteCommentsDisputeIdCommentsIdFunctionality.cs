using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.Functionality;

public class DeleteCommentsDisputeIdCommentsIdFunctionality
{
    private const string EndPoint = "/api/v1/comments/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendDeleteRequestDeleteCommentsDisputeIdCommentsIdAsync(
        RequestHeader requestHeader, string disputeId, string commentId)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeleteRequestDeleteCommentsDisputeIdCommentsId");
            var response = await _httpSender.SendDeleteHttpsRequestAsync(
                _envConfig.DisputesV2Url + EndPoint + disputeId + "/" + commentId, requestHeader);
            Console.WriteLine("Done with SendDeleteRequestDeleteCommentsDisputeIdCommentsId\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeleteRequestDeleteCommentsDisputeIdCommentsId \n" + exception + "\n");
            throw;
        }
    }
}