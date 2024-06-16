using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class OpenAuthorizationsFunctionality
{
    private const string OpenAuthorizationsEndPoint = "/api/installment-plan/open-authorizations";
    private readonly OpenAuthorizations.Root _openAuthorizations = new();
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseOpenAuthorizations.Root> SendPostRequestOpenAuthorizationsAsync(RequestHeader requestHeader, string ipn, bool isExecutedUnattended)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestOpenAuthorizations");
            _openAuthorizations.InstallmentPlanNumber = ipn;
            _openAuthorizations.IsExecutedUnattended = isExecutedUnattended;
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.AdminUrl + OpenAuthorizationsEndPoint,
                _openAuthorizations, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseOpenAuthorizations.Root>(response);
            if (jResponse!.Errors != null!)
            {
                for (var i = 0; i < 10; i++)
                {
                    response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.AdminUrl + OpenAuthorizationsEndPoint,
                        _openAuthorizations, requestHeader);
                    jResponse = JsonConvert.DeserializeObject<ResponseOpenAuthorizations.Root>(response);
                    if (jResponse!.Errors == null!)
                    {
                        return jResponse;
                    }
                }

                if (jResponse.Errors != null!)
                {
                    Console.WriteLine("Error in SendPostRequestOpenAuthorizations\n" + jResponse.Errors[0].Message + "\n");
                }
            }
            Console.WriteLine("Done with SendPostRequestOpenAuthorizations\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestOpenAuthorizations\n" + exception + "\n");
            throw;
        }
    }
}