using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Responses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminPortal.Logs.Functionality;

public class GetRequestResponseTraceIdFunctionality
{
    private const string EndPoint = "/api/v1/logs/request-response/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<GetRequestResponseTraceIdResponse.Root> SendGetRequestGetRequestResponseTraceIdAsync(
        RequestHeader requestHeader, string traceId)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetRequestResponseTraceIdAsync");
            var endPoint = _envConfig.AdminPortalApi + EndPoint + traceId;
            var response = await _httpSender.SendGetHttpsRequestAsync(endPoint, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<GetRequestResponseTraceIdResponse.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetRequestResponseTraceIdAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetRequestResponseTraceIdAsync \n" + exception + "\n");
            throw;
        }
    }
}