using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class RetryFunctionality
{
    private const string EndPoint = "/api/installment-plan/retry";
    private readonly HttpSender _httpSender = new();
    private readonly Retry.Root _retry = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponseRetry.Root> SendPostRequestRetryFunctionalityAsync(
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestRetryFunctionalityAsync");
            _retry.InstallmentPlanNumber = ipn;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.AdminUrl + EndPoint, _retry, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseRetry.Root>(response);
            Console.WriteLine("Done with SendPostRequestRetryFunctionalityAsync\n");
        return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestRetryFunctionalityAsync\n" + exception + "\n");
            throw;
        }
    }
}