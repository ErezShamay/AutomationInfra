using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class Get
{
    private const string GetEndPoint = "/api/InstallmentPlan/Get";
    private readonly HttpSender _httpSender = new();
    private readonly GetBaseObjects _getBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseGet.Root> SendGetRequestAsync(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(Environment.GetEnvironmentVariable("ApiV3")! + GetEndPoint,
                _getBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseGet.Root>(response);
            Console.WriteLine("Done with SendGetRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequest \n" + exception + "\n");
            throw;
        }
    }
}