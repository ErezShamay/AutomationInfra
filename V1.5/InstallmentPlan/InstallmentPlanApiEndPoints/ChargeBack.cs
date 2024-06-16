using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class ChargeBack
{
    private const string ChargeBackEndPoint = "/api/InstallmentPlan/ChargeBack";
    private readonly HttpSender _httpSender = new();
    private readonly ChargeBackBaseObjects _cancelBaseObjects = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseChargeBack.Root> SendChargeBackRequestAsync(RequestHeader requestHeader)
    {
        try
        {
            Console.WriteLine("\nStarting SendChargeBackRequest");
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + ChargeBackEndPoint,
                _cancelBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseChargeBack.Root>(response);
            Console.WriteLine("Done with SendChargeBackRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendChargeBackRequest \n" + exception + "\n");
            throw;
        }
    }
}