using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1.Login.LoginResponse;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1.Login.LoginApiEndPoints;

public class Login
{
    private const string LoginEndPoint = "/api/Login?format=json";
    private readonly HttpSender _httpSender = new();
    private readonly LoginBaseObjects.LoginBaseObjects.Login _loginProd = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<RequestHeader> DoLoginV1(RequestHeader requestHeader, string userName, string password, string apiKey)
    {
        try
        {
            Console.WriteLine("\nSending DoLoginV1");
            _loginProd.UserName = userName;
            _loginProd.Password = password;
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.BaseUrlProduction + LoginEndPoint, _loginProd, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseLogin.Root>(response);
            requestHeader.apiKey = apiKey;
            if (jResponse != null) requestHeader.sessionId = jResponse.SessionId;
            Console.WriteLine("DoLoginV1 Succeeded\n");
            return requestHeader;
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in DoLoginV1 \n" + exception + "\n");
            throw;
        }
    }
}