using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class UpdatePaymentData
{
    private const string UpdatePaymentDataEndPoint = "/api/InstallmentPlan/UpdatePaymentData";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseUpdatePaymentData.Root> SendUpdatePaymentDataRequestAsync(
        RequestHeader requestHeader, UpdatePaymentDataBaseObjects.Root updatePaymentDataBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendUpdatePaymentDataRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + UpdatePaymentDataEndPoint,
                updatePaymentDataBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseUpdatePaymentData.Root>(response);
            Console.WriteLine("Done with SendUpdatePaymentDataRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendUpdatePaymentDataRequest \n" + exception + "\n");
            throw;
        }
    }
}