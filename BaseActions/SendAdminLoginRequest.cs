using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class SendAdminLoginRequest
{
    private readonly HttpSender _httpSender = new();
    private readonly GetValueByKeyFromJson _getValueByKeyFromJson = new();

    public async Task<RequestHeader> DoAdminLogin(string accessTokenUri, string clientSecret, string splititMockTerminal, string clientId)
    {
        try
        {
            Console.WriteLine("\nSending DoAdminLogin");
            var requestHeader = new RequestHeader();
            var bodyReq = "grant_type=client_credentials&scope=reportingSystem.operation.api reportingSystem.config.api admin.api " +
                          "api.v2 api.v1 api.v15 api.v3 msgsystem.api gwmgmt.api embedded.api payfac.api funding.api retrieve.api idsrv merchantportal.api " +
                          "jobs.v2.mng jobs.v2.runner merchants.payments.api ams.api disputes.api keyexchange.api adminportal.api notifications.api" + 
                          "&client_id="+clientId+"&client_secret=" + clientSecret;
            var response = await _httpSender.SendPostHttpRequestStringBody(accessTokenUri + "/connect/token", bodyReq);
            requestHeader.apiKey = splititMockTerminal;
            requestHeader.sessionId = _getValueByKeyFromJson.GetValueOfObjectKey(response, "access_token");
            Console.WriteLine("DoAdminLogin Succeeded\n");
            return requestHeader;
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in DoAdminLogin \n" + exception + "\n");
            throw;
        }
    }
    
    public async Task<RequestHeader> DoClientLogin(string accessTokenUri, string clientSecret, string splititMockTerminal, string clientId)
    {
        try
        {
            Console.WriteLine("\nSending DoClientLogin");
            var requestHeader = new RequestHeader();
            var bodyReq = "grant_type=client_credentials&scope=admin.api api.v2 api.v1 disputes.api api.v3 " +
                          "keyexchange.api" + "&client_id="+clientId+"&client_secret=" + clientSecret;
            var response = await _httpSender.SendPostHttpRequestStringBody(accessTokenUri + "/connect/token", bodyReq);
            requestHeader.apiKey = splititMockTerminal;
            requestHeader.sessionId = _getValueByKeyFromJson.GetValueOfObjectKey(response, "access_token");
            Console.WriteLine("DoClientLogin Succeeded\n");
            return requestHeader;
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in DoClientLogin \n" + exception + "\n");
            throw;
        }
    }
    
    public async Task<RequestHeader> DoClientLoginKeyPair(RequestHeader requestHeaderOut, string accessTokenUri, 
        string clientSecret, string splititMockTerminal, string clientId)
    {
        try
        {
            Console.WriteLine("\nSending DoClientLogin");
            var requestHeader = new RequestHeader();
            var response = await _httpSender.SendPostRequestConnectTokenAsync(accessTokenUri + "/connect/token", requestHeaderOut,
                "client_credentials", "admin.api api.v2 api.v1 disputes.api api.v3 keyexchange.api",
                clientId, clientSecret);
            requestHeader.apiKey = splititMockTerminal;
            requestHeader.sessionId = _getValueByKeyFromJson.GetValueOfObjectKey(response, "access_token");
            Console.WriteLine("DoClientLogin Succeeded\n");
            return requestHeader;
        }
        catch(Exception exception)
        {
            Console.WriteLine("Error in DoClientLogin \n" + exception + "\n");
            throw;
        }
    }
}