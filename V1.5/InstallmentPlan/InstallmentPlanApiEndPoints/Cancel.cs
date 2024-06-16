using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class Cancel
{
    private const string CancelEndPoint = "/api/InstallmentPlan/Cancel";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponseCancel.Root> SendCancelRequestAsync(RequestHeader requestHeader, CancelBaseObjects.Root cancelBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendCancelRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + CancelEndPoint,
                cancelBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseCancel.Root>(response);
            Console.WriteLine("Done with SendCancelRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendCancelRequest \n" + exception + "\n");
            throw;
        }
    }
}