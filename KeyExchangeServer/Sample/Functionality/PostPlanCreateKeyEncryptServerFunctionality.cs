using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.KeyExchangeServer.Sample.Functionality;

public class PostPlanCreateKeyEncryptServerFunctionality
{
    private const string EndPoint = "/api/installmentplans";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseV3.ResponseRoot?> SendPostPlanCreateKeyEncryptServerAsync(
        RequestHeader requestHeader, string bodyToSend, string kesFlag = default!, string requestSignature = default!,
        string requestSignatureKeyId = default!, string requestSignatureClientId = default!)
    {
        try
        {
            Console.WriteLine("Starting SendPostPlanCreateKeyEncryptServer");
            string response;
            if (kesFlag != null!)
            {
                response = await _httpSender.SendPostRequestStringBody(
                    Environment.GetEnvironmentVariable("ApiV3")! + EndPoint, requestHeader, bodyToSend, "yes"
                    , requestSignature, requestSignatureKeyId, requestSignatureClientId);
            }
            else
            {
                response = await _httpSender.SendPostRequestStringBody(
                    Environment.GetEnvironmentVariable("ApiV3")! + EndPoint, requestHeader, bodyToSend);
            }
            var jResponse = JsonConvert.DeserializeObject<ResponseV3.ResponseRoot>(response);
            Console.WriteLine("Done with SendPostPlanCreateKeyEncryptServer");
            return jResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in SendPostPlanCreateKeyEncryptServer" + e);
            throw;
        }
    }
}