using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.PlanCreate.Response;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.PlanCreate.Functionality;

public class DispatchDrPlanFunctionality
{
    private const string EndPoint = "/api/v1/plan/dispatch-dr-plans";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<DispatchDrPlanResponse.Root> SendPostRequestDispatchDrPlanAsync(
        RequestHeader requestHeader, string batchSize, string delayMs)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestDispatchDrPlan");
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.PlanCreateApiUrl + EndPoint + "?batchSize="+batchSize+"&delayMs="+delayMs,
                null!, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<DispatchDrPlanResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestDispatchDrPlan\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestDispatchDrPlan \n" + exception + "\n");
            throw;
        }
    }
}