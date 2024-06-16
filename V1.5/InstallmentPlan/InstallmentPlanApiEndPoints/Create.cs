using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class Create
{
    private const string CreatePlanEndPoint = "/api/InstallmentPlan/Create";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponseCreate.Root> CreatePlanV1Point5Async(RequestHeader requestHeader, 
        int numberOfInstallments, CreatePlanV1Point5DefaultValues createPlanV1Point5DefaultValues, string? googleFlag = default!, string? idempotencyKey = default!)
    {
        try
        {
            Console.WriteLine("Starting CreatePlanV1Point5");
            createPlanV1Point5DefaultValues.planData.numberOfInstallments = numberOfInstallments;
            createPlanV1Point5DefaultValues.RequestHeader.apiKey = requestHeader.apiKey;
            createPlanV1Point5DefaultValues.RequestHeader.touchPoint = requestHeader.touchPoint;
            string response;
            if (googleFlag != null)
            {
                response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + CreatePlanEndPoint,
                    createPlanV1Point5DefaultValues, requestHeader, googleFlag, idempotencyKey);
            }
            else
            {
                response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + CreatePlanEndPoint,
                    createPlanV1Point5DefaultValues, requestHeader);
            }
            var jResponse = JsonConvert.DeserializeObject<ResponseCreate.Root>(response);
            Console.WriteLine("Done with CreatePlanV1Point5");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in CreatePlanV1Point5 -> " + exception);
            throw;
        }
    }
}