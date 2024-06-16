using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.WebApi.InstallmentPlan.BaseObjects;
using Splitit.Automation.NG.Backend.Services.WebApi.InstallmentPlan.Responses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.WebApi.InstallmentPlan.Functionality;

public class PostTermsAndConditionsFunctionality
{
    private const string EndPoint = "/api/InstallmentPlan/TermsAndConditions";
    private readonly PostTermsAndConditionsBaseObjects.Root _root = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<PostTermsAndConditionsResponse.Root> SendPostRequestPostTermsAndConditionsAsync(
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestPostTermsAndConditionsAsync");
            _root.InstallmentPlanNumber = ipn;
            var response = await _httpSender.SendPostHttpsRequestAsync(
                _envConfig.WebApiDockerUrl + EndPoint,
                _root, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<PostTermsAndConditionsResponse.Root>(response);
            Console.WriteLine("Done with SendPostRequestPostTermsAndConditionsAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestPostTermsAndConditionsAsync\n" + exception + "\n");
            throw;
        }
    }
}