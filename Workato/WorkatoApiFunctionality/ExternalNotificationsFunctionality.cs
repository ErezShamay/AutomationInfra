using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.Ams.Workato.WorkatoBaseObjects;
using Splitit.Automation.NG.Backend.Services.Ams.Workato.WorkatoResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.Ams.Workato.WorkatoApiFunctionality;

public class ExternalNotificationsFunctionality
{
    private const string ExternalNotificationEndPoint = "/api/v1/external-notifications";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ExternalNotificationsResponse.Root> PostRequestToCreatePlanExternalNotifications(RequestHeader requestHeader, ExternalNotificationsBaseObjects externalNotificationsBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting PostRequestToCreatePlanExternalNotifications");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.Ams + ExternalNotificationEndPoint, externalNotificationsBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ExternalNotificationsResponse.Root>(response);
            if (Enumerable.Range(400,499).Contains(jResponse!.StatusCode))
            {
                Assert.Fail("Error in PostRequestToCreatePlanExternalNotifications -> with status code -> " + jResponse.StatusCode);
            }
            else if (Enumerable.Range(500,599).Contains(jResponse!.StatusCode))
            {
                Assert.Fail("Error in PostRequestToCreatePlanExternalNotifications -> with status code -> " + jResponse.StatusCode);
            }
            Console.WriteLine("Done with PostRequestToCreatePlanExternalNotifications");
            return jResponse;
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in PostRequestToCreatePlanExternalNotifications" + exception + "\n");
            throw;
        }
    }
}