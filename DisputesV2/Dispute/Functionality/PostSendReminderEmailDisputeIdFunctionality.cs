using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.BaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.DisputesV2.Dispute.Functionality;

public class PostSendReminderEmailDisputeIdFunctionality
{
    private const string EndPoint = "/api/v1/disputes/send-reminder-email/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    private readonly PostSendReminderEmailDisputeIdBaseObjects _postSendReminderEmailDisputeIdBaseObjects = new();

    public async Task<string> SendPostRequestPostSendReminderEmailDisputeIdAsync(
        RequestHeader requestHeader, string disputeId)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostSendReminderEmailDisputeId");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.DisputesV2Url + EndPoint + disputeId,
                _postSendReminderEmailDisputeIdBaseObjects, requestHeader);
            //var jResponse = JsonConvert.DeserializeObject<PostDisputesResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostSendReminderEmailDisputeId\n");
            return response!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostSendReminderEmailDisputeId \n" + exception + "\n");
            throw;
        }
    }
}