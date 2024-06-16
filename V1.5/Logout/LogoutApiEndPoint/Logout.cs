using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.Logout.LogoutBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1._5.Logout.LogoutResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.V1._5.Logout.LogoutApiEndPoint;

public class Logout
{
    private readonly HttpSender _httpSender = new();
    private const string LoginEndPoint = "/api/Logout?format=json";
    private readonly LogoutBaseObject _logoutBaseObject = new();

    public async Task<LogoutResponse.Root> SendLogoutRequestAsync(string baseUrl, RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("Starting SendLogoutRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(baseUrl + LoginEndPoint, _logoutBaseObject, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<LogoutResponse.Root>(response);
            Console.WriteLine("Done with SendLogoutRequest");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendLogoutRequest functionality \n" + exception);
            throw;
        }
    }
}