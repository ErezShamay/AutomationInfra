using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class ChargeFunctionality
{
    private const string ChargeEndPoint = "/api/installment-plan/charge";
    private readonly Charge.Root _charge = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseCharge.Root> SendPostRequestChargeFunctionalityAsync(RequestHeader requestHeader,
        string ipn, bool isNegativeTest = false)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestChargeFunctionality");
            await Task.Delay(3 * 1000);
            _charge.InstallmentPlanNumber = ipn;
            _charge.ChargeMethod = "Default";
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.AdminUrl + ChargeEndPoint,
                _charge, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseCharge.Root>(response);
            if (isNegativeTest && jResponse!.Errors != null!)
            {
                return jResponse;
            }
            if (isNegativeTest && jResponse!.Errors == null!)
            {
                Assert.Fail("Charge did not failed as expected\n");
            }
            if (jResponse!.Errors != null!)
            {
                Console.WriteLine("Doing retry for charge");
                await Task.Delay(3 * 1000);
                var responseRetry = await _httpSender.SendPostHttpsRequestAsync(_envConfig.AdminUrl + ChargeEndPoint,
                    _charge, requestHeader);
                var jResponseRetry = JsonConvert.DeserializeObject<ResponseCharge.Root>(responseRetry);
                if (jResponseRetry!.Errors != null!)
                {
                    Assert.Fail("Charge retry FAILED\n");
                }

                return jResponseRetry;
            }
            Console.WriteLine("Done with SendPostRequestChargeFunctionality\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestChargeFunctionality\n" + exception + "\n");
            throw;
        }
    }
}