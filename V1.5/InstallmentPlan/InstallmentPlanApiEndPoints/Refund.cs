using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V1._5.InstallmentPlan.InstallmentPlanApiEndPoints;

public class Refund
{
    private const string RefundPlanEndPoint = "/api/InstallmentPlan/Refund";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseRefund.Root> SendRefundRequestAsync(RequestHeader requestHeader,
        decimal amount, int refundStrategy, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendRefundRequest");
            await Task.Delay(10*1000);
            var refundBaseObjects = new RefundBaseObjects.Root();
            var amountObject = new RefundBaseObjects.Amount
            {
                value = amount
            };
            refundBaseObjects.refundStrategy = refundStrategy;
            refundBaseObjects.amount = amountObject;
            refundBaseObjects.installmentPlanNumber = ipn;
            var response = await _httpSender.SendPostHttpsRequestAsync(_envConfig.GoogleBaseUri + RefundPlanEndPoint,
                refundBaseObjects, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseRefund.Root>(response);
            Console.WriteLine("Done with SendRefundRequest\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendRefundRequest \n" + exception + "\n");
            throw;
        }
    }
}