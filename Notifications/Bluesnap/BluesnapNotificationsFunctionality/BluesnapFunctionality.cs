using Splitit.Automation.NG.Backend.BaseActions;

namespace Splitit.Automation.NG.Backend.Services.Notifications.Bluesnap.BluesnapNotificationsFunctionality;

public class BluesnapFunctionality
{
    private readonly HttpSender _httpSender = new();
    private const string BluesnapNotificationsEndPoint = "/api/notifications/bluesnap";

    private string CreateRequestBody(string chargeId, string transactionType, string cbStatus, string reversalReason)
    {
        var untilDate = DateTime.Now.AddDays(3).ToString("MM/dd/yyyy hh:mm tt");
        var transactionDate = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy hh:mm tt");
        var requestBody = "title=Mabas&" +
                             "transactionType="+transactionType+"&" +
                             "untilDate="+untilDate+"&" +
                             "reversalRefNum=" + $"{chargeId}" + "&" +
                             "reversalReason="+reversalReason+"&" +
                             "referenceNumber="+chargeId+"&" +
                             "invoiceAmount=" + "1" + "&" +
                             "transactionDate="+transactionDate+"&" +
                             "cbStatus="+cbStatus+"";
        return requestBody;
    }
    
    public async Task<string> SendPostRequestBluesnapNotifications(string baseUrl, string captureId, string transactionType, string cbStatus, string reversalReason)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestBluesnapNotifications");
            var requestBody = CreateRequestBody(captureId, transactionType, cbStatus, reversalReason);
            var response = await _httpSender.SendPostHttpRequestStringBody(baseUrl + BluesnapNotificationsEndPoint, requestBody);
            Console.WriteLine("Done with SendPostRequestBluesnapNotifications\n");
            
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestBluesnapNotifications \n" + exception + "\n");
            throw;
        }
    }
}