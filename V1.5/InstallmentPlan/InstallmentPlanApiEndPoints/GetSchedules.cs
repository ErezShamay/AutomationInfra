using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class GetSchedules
{
    private const string GetSchedulesEndPoint = "/api/InstallmentPlan/GetSchedules";
    private readonly HttpSender _httpSender = new();
    private readonly GetSchedulesBaseObjects _getSchedulesBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseCancel.Root> SendGetSchedulesRequestAsync(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetSchedulesRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + GetSchedulesEndPoint,
                _getSchedulesBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseCancel.Root>(response);
            Console.WriteLine("Done with SendGetSchedulesRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetSchedulesRequest \n" + exception + "\n");
            throw;
        }
    }
}