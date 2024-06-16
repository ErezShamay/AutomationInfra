using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.V1._5.Login.LoginApiEndPoints;

public class Login
{
    private readonly HttpSender _httpSender = new();
    private readonly Dictionary<string, string> _loginBody = new ();
    private readonly GetValueByKeyFromJson _getValueByKeyFromJson = new();
    private const string LoginEndPoint = "/api/Login?format=json";

    public async Task<RequestHeader> LoginAsMerchantAsync(string baseUrl, RequestHeader requestHeader, string mockTerminal, string userName, string pass)
    {
        try
        {
            Console.WriteLine("Starting LoginAsMerchant");
            _loginBody.Add("userName", userName);
            _loginBody.Add("password", pass);
            var response = await _httpSender.SendPostHttpsRequestAsync(baseUrl + LoginEndPoint, _loginBody, requestHeader);
            requestHeader.apiKey = mockTerminal;
            requestHeader.sessionId = _getValueByKeyFromJson.GetValueOfObjectKey(response, "SessionId");
            Console.WriteLine("Done with LoginAsMerchant");
            return requestHeader;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in LoginAsMerchant functionality \n" + exception);
            throw;
        }
    }
}