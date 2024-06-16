using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsHelper;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class FullPlanInfoIpn
{
    private const string GetFullPlanInfoByIpn = "/api/installment-plan/full-plan-info/";
    private readonly HttpSender _httpSender = new();
    private int _counter;
    private readonly EnvConfig _envConfig = new();
    private readonly TestsHelper _testsHelper = new();

    public async Task<ResponseFullPlanInfoIpn.Root> SendGetFullPlanInfoIpnAsync(RequestHeader requestHeader, 
        string ipn, string isNegativeTest = default!, string pgtlCaptureTransactionId = default!,
        List<string> gatewayEventTypeList = default!)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetFullPlanInfoIpn");
            await Task.Delay(5*1000);
            var getFullPlanInfoByIpn = GetFullPlanInfoByIpn + ipn;
            var response = await _httpSender.SendGetHttpsRequestAsync(_envConfig.AdminUrl + getFullPlanInfoByIpn,
                    requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseFullPlanInfoIpn.Root>(response);
            if (isNegativeTest != null!)
            {
                return jResponse!;
            }
            
            if (jResponse!.InstallmentPlan == null!)
            {
                while (_counter < 20)
                {
                    try
                    {
                        await Task.Delay(5*1000);
                        _counter++;
                        response = await _httpSender.SendGetHttpsRequestAsync(
                            _envConfig.AdminUrl + getFullPlanInfoByIpn, requestHeader);
                        jResponse = JsonConvert.DeserializeObject<ResponseFullPlanInfoIpn.Root>(response);
                        if (jResponse!.InstallmentPlan != null!)
                        {
                            return jResponse;
                        }
                    }
                    catch (Exception exceptionIn)
                    {
                        Console.WriteLine("Installment plan is still Null retry\n" + exceptionIn + "\n");
                    }
                }
            }

            Console.WriteLine("Validating response include full installment plan info...");
            if (jResponse!.InstallmentPlan == null)
            {
                Assert.Fail("Response DOES NOT includes full installment plan info");
            }

            if (pgtlCaptureTransactionId != null!)
            {
                if (!await _testsHelper.CheckWebhookInCollection(pgtlCaptureTransactionId, gatewayEventTypeList))
                {
                    Assert.Fail("DID NOT find webhook notification from the gateway");
                }
            }
            Console.WriteLine("Response does includes full installment plan info");
            Console.WriteLine("Done with SendGetFullPlanInfoIpn\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetFullPlanInfoIpn\n" + exception + "\n");
            throw;
        }
    }
}