using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class ReAuthFunctionality
{
    private const string ReAuthEndPoint = "/api/installment-plan/re-auth";
    private readonly ReAuth.Root _reAuth = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseReAuth.Root> SendPostRequestReAuthAsync(RequestHeader requestHeader, string ipn, string negativeFlag = default!)
    {
        try
        {
            await Task.Delay(10 * 1000);
            ResponseReAuth.Root? jResponse = null!;
            for (var retryCount = 0; retryCount < 10; retryCount++)
            {
                Console.WriteLine("\nStarting SendPostRequestReAuth");
                _reAuth.InstallmentPlanNumber = ipn;
                var response = await _httpSender.SendPostHttpsRequestAsync(
                    _envConfig.AdminUrl + ReAuthEndPoint,
                    _reAuth, requestHeader);
                jResponse = JsonConvert.DeserializeObject<ResponseReAuth.Root>(response);
                if (negativeFlag != null)
                {
                    return jResponse!;
                }
                if (jResponse!.ResponseHeader.Succeeded)
                {
                    return jResponse;
                }
                await Task.Delay(5 * 1000);
            }
            Console.WriteLine("Done with SendPostRequestReAuth\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestReAuth\n" + exception + "\n");
            throw;
        }
    }
}