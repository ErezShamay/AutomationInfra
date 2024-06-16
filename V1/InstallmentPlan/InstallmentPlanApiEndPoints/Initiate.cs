using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;

public class Initiate
{
    private const string CreatePlanInitEndPoint = "/api/InstallmentPlan/Initiate";
    private readonly HttpSender _httpSender = new();
    
    public async Task<InitiateResponse.Root?> CreatePlanInitiate(string baseUrl, RequestHeader requestHeader,
        int numberOfInstallments, V1InitiateDefaultValues v1InitiateDefaultValues)
    {
        try
        {
            Console.WriteLine("\nStarting CreatePlanInitiate");
            v1InitiateDefaultValues.planData.numberOfInstallments = numberOfInstallments;
            var response = await _httpSender.SendPostHttpsRequestAsync(baseUrl + CreatePlanInitEndPoint, v1InitiateDefaultValues, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<InitiateResponse.Root>(response);
            Console.WriteLine("CreatePlan Succeeded! IPN -> " + jResponse?.InstallmentPlan.InstallmentPlanNumber +
                              " With Status -> " + jResponse?.InstallmentPlan.InstallmentPlanStatus.Description + "\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanInitiate\n" + exception + "\n");
            throw;
        }
    }
}