using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.BaseObjects;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Comments.Functionality;

public class PostCommentsDisputeIdFunctionality
{
    private const string EndPoint = "/api/v1/comments/";
    private readonly HttpSender _httpSender = new();
    private readonly PostCommentsDisputeIdBaseObjects.Root _postCommentsDisputeIdBaseObjects = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<PostCommentsDisputeIdResponse.Root> SendPostCommentsDisputeIdRequestAsync(
        RequestHeader requestHeader, string disputeId, string text, string createdBy, string? commentId, bool internalComment)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostCommentsDisputeIdRequest");
            _postCommentsDisputeIdBaseObjects.CommentId = commentId!;
            _postCommentsDisputeIdBaseObjects.InternalComment = internalComment;
            _postCommentsDisputeIdBaseObjects.Text = text;
            _postCommentsDisputeIdBaseObjects.CreatedBy = createdBy;
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint + disputeId,
                _postCommentsDisputeIdBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostCommentsDisputeIdResponse.Root>(response);
            Console.WriteLine("Done with SendPostCommentsDisputeIdRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostCommentsDisputeIdRequest \n" + exception + "\n");
            throw;
        }
    }
}