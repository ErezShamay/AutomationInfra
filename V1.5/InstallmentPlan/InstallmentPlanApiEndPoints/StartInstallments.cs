using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class StartInstallments
{
    private const string StartInstallmentsPlanEndPoint = "/api/InstallmentPlan/StartInstallments";
    private readonly HttpSender _httpSender = new();
    private readonly StartInstallmentsBaseObjects _startInstallmentsBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseStartInstallments.Root> SendStartInstallmentsRequestAsync(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendStartInstallmentsRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + StartInstallmentsPlanEndPoint,
                _startInstallmentsBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseStartInstallments.Root>(response);
            Console.WriteLine("Done with SendStartInstallmentsRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendStartInstallmentsRequest \n" + exception + "\n");
            throw;
        }
    }
}