using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;

public class GetInstallmentPlan
{
    private const string EndPoint = "/api/installmentplans/";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseV3GetInstallmentPlan.Root> SendGetRequestGetInstallmentPlanAsync(
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetInstallmentPlan");
            var response = await _httpSender.SendGetHttpsRequestAsync(Environment.GetEnvironmentVariable("ApiV3")! + EndPoint + ipn, 
                requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseV3GetInstallmentPlan.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetInstallmentPlan\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetInstallmentPlan \n" + exception + "\n");
            throw;
        }
    }
}