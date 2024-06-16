using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class FullCaptureFunctionality
{
    private const string FullCaptureEndPoint = "/api/installment-plan/full-capture";
    private readonly FullCapture.Root _fullCapture = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseFullCapture.Root> SendPostRequestFullCaptureAsync(RequestHeader requestHeader, string ipn, int reason)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestFullCapture");
            await Task.Delay(10 * 1000);
            _fullCapture.InstallmentPlanNumber = ipn;
            _fullCapture.FullCaptureReason = reason;
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.AdminUrl + FullCaptureEndPoint, _fullCapture, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseFullCapture.Root>(response);
            if (jResponse!.Errors != null!)
            {
                Console.WriteLine("retrying to do full capture --> Error is: " + jResponse.ResponseHeader.Errors[0].ErrorCode + " " +
                                  jResponse.ResponseHeader.Errors[0].Message);
                for (var i = 0; i < 10; i++)
                {
                    await Task.Delay(5 * 1000);
                    response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.AdminUrl + FullCaptureEndPoint,
                        _fullCapture, requestHeader);
                    jResponse = JsonConvert.DeserializeObject<ResponseFullCapture.Root>(response);
                    if (jResponse!.ResponseHeader.Succeeded)
                    {
                        return jResponse;
                    }
                }
            }
            Console.WriteLine("Done with SendPostRequestFullCapture\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestFullCapture\n" + exception + "\n");
            throw;
        }
    }
}