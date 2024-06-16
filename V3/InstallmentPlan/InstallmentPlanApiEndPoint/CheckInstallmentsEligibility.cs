using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;

public class CheckInstallmentsEligibility
{
    private const string CheckEligibilityEndPoint = "/api/installmentplans/check-eligibility";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<ResponseV3CheckEligibility.Root> SendCheckEligibilityRequestAsync(RequestHeader requestHeader, string terminal,
        CheckEligibilityDefaultValues checkEligibility)
    {
        try
        {
            Console.WriteLine("\nStarting SendCheckEligibilityRequest");
            checkEligibility.planData.terminalId = terminal;
            var response = await _httpSender.SendPostHttpsRequestAsync(Environment.GetEnvironmentVariable("ApiV3")! + CheckEligibilityEndPoint,
                checkEligibility, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseV3CheckEligibility.Root>(response);
            Console.WriteLine("Done with SendCheckEligibilityRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendCheckEligibilityRequest \n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateCheckEligibility(ResponseV3CheckEligibility.Root jResponse)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateCheckEligibility");
            var counter = jResponse.PaymentPlanOptions.Count;
            for (var i = 1; i <= jResponse.PaymentPlanOptions.Count; i++)
            {
                
                Assert.That(jResponse.PaymentPlanOptions[counter-1].Links.LearnMoreUrl.Contains(i.ToString()));
                counter--;
            }
            Console.WriteLine("Done ValidateCheckEligibility\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateCheckEligibility \n" + exception + "\n");
            return false;
        }
    }
}