using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Functionality;

public class GetEmailsFunctionality
{
    private const string EndPoint = "/api/v1/logs/emails";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetEmailsResponse.Root> SendGetRequestGetEmailsAsync(
        RequestHeader requestHeader, string searchByKey, string searchByValue, bool expectingOneEmail = false)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetEmailsAsync");
            GetEmailsResponse.Root? parsedResponse = null;
            var endPoint = 
                _envConfig.AdminPortalApi + EndPoint + "?" + searchByKey + "=" + searchByValue;
            for (var i = 0; i < 80; i++)
            {
                await Task.Delay(5 * 1000);
                var response = await _httpSender.SendGetHttpsRequestAsync(endPoint, requestHeader);
                parsedResponse = JsonConvert.DeserializeObject<GetEmailsResponse.Root>(response);
                if (expectingOneEmail && parsedResponse!.Logs.Count > 0)
                {
                    return parsedResponse;
                }
                if (parsedResponse!.Logs.Count > 1)
                {
                    return parsedResponse;
                }
            }
            Console.WriteLine("Done with SendGetRequestGetEmailsAsync\n");
            return parsedResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetEmailsAsync \n" + exception + "\n");
            throw;
        }
    }
}