using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;

public class InstallmentPlanNumberRefund
{
    private const string CreatePlanEndpoint = "/api/installmentplans";
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseV3Refund.Root> SendRefundRequestAsync(RequestHeader requestHeader, string ipn
        , string refundType, string refundStrategy = null!, string isNegativeTest = null!, int amountToRefund = default )
    {
        try
        {
            Console.WriteLine("\nStarting SendRefundRequest");
            var httpSender = new HttpSender();
            var refundEndPoint = CreatePlanEndpoint + "/" + ipn + "/refund?type=json";
            var refund = new RefundDefaultValues.Root();
            refund.Amount = refundType switch
            {
                "FullRefund" => 0,
                "ExceededAmount" => 9999999,
                "PartialRefund" => 1,
                "FutureInstallmentsLast" => 1,
                "ReduceFromLastInstallment" => 1,
                "AmountToRefund" => amountToRefund,
                _ => refund.Amount
            };
            if (refundStrategy != null!)
            {
                refund.RefundStrategy = refundStrategy;
            }
            else
            {
                refund.RefundStrategy = "FutureInstallmentsFirst";
            }
            var response = await httpSender.SendPostHttpsRequestAsync(_envConfig.ApiV3 + refundEndPoint, 
                refund, requestHeader);
            var responseRefund = JsonConvert.DeserializeObject<ResponseV3Refund.Root>(response);
            if (isNegativeTest != null!) return responseRefund!;
            if (responseRefund!.RefundId == null! || responseRefund.Error != null!)
            {
                Console.WriteLine("Refund did not succeed -> RefundId field is null -> doing retry");
                for (var i = 0; i < 5; i++)
                {
                    await Task.Delay(5 * 1000);
                    response = await httpSender.SendPostHttpsRequestAsync(_envConfig.ApiV3 + refundEndPoint,
                        refund, requestHeader);
                    responseRefund = JsonConvert.DeserializeObject<ResponseV3Refund.Root>(response)!;
                    if (ValidateRefundResponse(responseRefund))
                    {
                        return responseRefund;
                    }
                }
            }
            if (responseRefund.Summary.FailedAmount > 0)
            {
                Assert.Fail("Refund did not succeeded -> FailedAmount field is not 0");
            }
            Console.WriteLine("Refund Succeeded! IPN -> " + responseRefund.InstallmentPlanNumber + " With RefundID -> " + responseRefund.RefundId);
            Console.WriteLine("SendRefundRequest Succeeded\n");

            return responseRefund;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendRefundRequest Failed \n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateRefund(ResponseV3Refund.Root responseRefund, string ipn)
    {
        try
        {
            Console.WriteLine("\nValidating refund id in not null");
            Assert.That(responseRefund.RefundId, Is.Not.Null);
            Console.WriteLine("Validated! refund id in not null");
            Console.WriteLine("Validating ipn from plan creation is the same as in the refund response");
            Assert.That(ipn, Is.EqualTo(responseRefund.InstallmentPlanNumber));
            Console.WriteLine("Validated! ipn from plan creation is the same as in the refund response");
            Console.WriteLine("ValidateRefund Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateRefund Failed\n" + exception + "\n");
            return false;
        }
    }

    private bool ValidateRefundResponse(ResponseV3Refund.Root responseRefund)
    {
        try
        {
            Console.WriteLine("Starting ValidateRefundResponse");
            if(responseRefund.Error != null!)
            {
                Console.WriteLine("jResponse.Error is not null the error message is --->>>> " + responseRefund.Error.Message);
                return true;
            }

            if (responseRefund.Summary != null!)
            {
                if (responseRefund.Summary.FailedAmount > 0)
                {
                    Assert.Fail("Refund did not succeeded -> FailedAmount field is not 0");
                }
            }
            else
            {
                Console.WriteLine("responseRefund.Summary is null continue the retry");
                return false;
            }
            Console.WriteLine("Refund Succeeded for IPN -> " + responseRefund.InstallmentPlanNumber +
                              " With RefundID -> " + responseRefund.RefundId);
            Console.WriteLine("Done with ValidateRefundResponse");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateRefundResponse" + e);
            throw;
        }
    }
}