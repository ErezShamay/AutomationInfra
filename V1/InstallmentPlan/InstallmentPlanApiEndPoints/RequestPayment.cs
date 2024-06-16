using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;

public class RequestPayment
{
    private const string EndPoint = "/api/InstallmentPlan/RequestPayment";
    private readonly RequestPaymentBaseObjects.Root _root = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<RequestPaymentResponse.Root> SendPostRequestRequestPaymentAsync(
        RequestHeader requestHeader, string ipn, string approvalEmail,
        string touchPointCode, string touchPointVersion)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestRequestPaymentAsync");
            _root.InstallmentPlanNumber = ipn;
            _root.PaymentApprovalEmail = approvalEmail;
            _root.requestHeader = new RequestHeader
            {
                touchPoint = new TouchPoint
                {
                    code = touchPointCode,
                    version = touchPointVersion
                },
                sessionId = requestHeader.sessionId,
                apiKey = requestHeader.apiKey
            };
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.StoreProcedureUrl + EndPoint,
                _root, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<RequestPaymentResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestRequestPaymentAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestRequestPaymentAsync\n" + exception + "\n");
            throw;
        }
    }
}