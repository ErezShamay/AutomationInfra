using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class VerifyPayment
{
    private const string VerifyPaymentEndPoint = "/api/InstallmentPlan/Get/VerifyPayment";
    private readonly HttpSender _httpSender = new();
    private readonly VerifyPaymentBaseObjects _verifyPaymentBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseVerifyPayment.Root> SendVerifyPaymentRequestAsync(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendVerifyPaymentRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + VerifyPaymentEndPoint,
                _verifyPaymentBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseVerifyPayment.Root>(response);
            Console.WriteLine("Done with SendVerifyPaymentRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendVerifyPaymentRequest \n" + exception + "\n");
            throw;
        }
    }
}