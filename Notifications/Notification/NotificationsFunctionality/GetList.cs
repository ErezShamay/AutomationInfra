using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Notifications.Notification.NotificationsResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Notifications.Notification.NotificationsFunctionality;

public class GetList
{
    private const string EndPoint = "/api/Notifications/list";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<GetListResponse.Root> SendGetRequestGetNotificationsListAsync(
        RequestHeader requestHeader, string queryParamName, string queryParamValue)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetNotificationsListAsync");
            var response = await _httpSender.SendGetHttpsRequestAsync(
                _envConfig.NotificationUrl + EndPoint + "?" + queryParamName + "=" + queryParamValue, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetListResponse.Root>(response);
            if (jResponse!.webhooks.Count == 0)
            {
                Console.WriteLine("Starting Retry for SendGetRequestGetNotificationsListAsync");
                GetListResponse.Root jResponseRetry = null!;
                for (var i = 0; i < 60; i++)
                {
                    await Task.Delay(5 * 1000);
                    var responseRetry = await _httpSender.SendGetHttpsRequestAsync(
                        _envConfig.NotificationUrl + EndPoint + "?" + queryParamName + "=" + queryParamValue,
                        requestHeader);
                    jResponseRetry = JsonConvert.DeserializeObject<GetListResponse.Root>(responseRetry)!;
                    if (jResponseRetry.webhooks.Count > 0)
                    {
                        return jResponseRetry;
                    }
                }

                if (jResponseRetry.webhooks.Count == 0)
                {
                    Assert.Fail("no webhooks found in the response after retrying 15 times");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Done with SendGetRequestGetNotificationsListAsync\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetNotificationsListAsync \n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateGatewayEventType(GetListResponse.Root listResponse, string eventType)
    {
        try
        {
            Console.WriteLine("Starting ValidateGatewayEventType");
            foreach (var webhook in listResponse.webhooks)
            {
                if (!webhook.gatewayEventType.Equals(eventType)) return false;
                Console.WriteLine("eventType -> " + eventType + " validated");
                return true;

            }

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateGatewayEventType" + e);
            throw;
        }
    }
}