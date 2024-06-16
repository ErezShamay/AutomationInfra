using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class RefundFunctionality
{
     private const string RefundEndpoint = "/api/installment-plan/refund";
     private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseRefund.Root> SendRefundRequestAsync(RequestHeader requestHeader, string ipn
        , string refundType, string refundStrategy = null!, string isNegativeTest = null!, double amountToRefund = default )
    {
        string responseRetry;
        ResponseRefund.Root? jResponseRetry = null;
        
        try
        {
            Console.WriteLine("\nStarting SendRefundRequest");
            await Task.Delay(10*1000);
            var httpSender = new HttpSender();
            var refund = new Refund.Root();
            var amount = new Refund.Amount();
            refund.InstallmentPlanNumber = ipn;
            amount.Value = refundType switch
            {
                "FullRefund" => 0,
                "ExceededAmount" => 9999999,
                "PartialRefund" => 1,
                "FutureInstallmentsLast" => 1,
                "ReduceFromLastInstallment" => 1,
                "AmountToRefund" => amountToRefund,
                _ => amount.Value
            };
            refund.Amount = amount;
            if (refundStrategy != null)
            {
                refund.RefundStrategy = refundStrategy;
            }
            else
            {
                refund.RefundStrategy = "FutureInstallmentsFirst";
            }
            var response = await httpSender.SendPostHttpsRequestAsync(_envConfig.AdminUrl + RefundEndpoint, refund, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseRefund.Root>(response);
            if (isNegativeTest == null)
            {
                if (jResponse!.InstallmentPlan.OriginalAmount == null!)
                {
                    Console.WriteLine("Refund did not succeeded -> RefundId field is null doing retry");
                    for (var i = 0; i < 5; i++)
                    {
                        await Task.Delay(5*1000);
                        responseRetry =
                            await httpSender.SendPostHttpsRequestAsync(Environment.GetEnvironmentVariable("ApiV3")! + RefundEndpoint,
                                refund, requestHeader);
                        jResponseRetry = JsonConvert.DeserializeObject<ResponseRefund.Root>(responseRetry)!;
                        if (jResponseRetry.InstallmentPlan.OriginalAmount == null!) continue;
                        Console.WriteLine("Refund Succeeded! for IPN -> " + jResponseRetry.InstallmentPlan.InstallmentPlanNumber);
                        return jResponseRetry;
                    }

                    if (jResponseRetry!.InstallmentPlan.OriginalAmount == null!)
                    {
                        Assert.Fail("Refund did not succeeded -> RefundId field is null after 5 retry");
                    }
                }
                Console.WriteLine("Refund Succeeded! for IPN -> " + jResponse.InstallmentPlan.InstallmentPlanNumber);
                Console.WriteLine("SendRefundRequest Succeeded\n");
            }
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendRefundRequest Failed \n" + exception + "\n");
            throw;
        }
    }

}