using Newtonsoft.Json;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanApiEndPoint;

public class InstallmentPlanNumberUpdateOrder
{
    private const string CreatePlanEndpoint = "/api/installmentplans";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseV3Update.Root?> SendUpdateRequestAsync(RequestHeader requestHeader,
        string ipn, string status, bool capture, Identifier identifier)
    {
        var counter = 0;
        try
        {
            Console.WriteLine("\nStarting SendUpdateRequest");
            await Task.Delay(15*1000);
            var updateEndPoint = CreatePlanEndpoint + "/" + ipn + "/updateorder";
            var updateOrder = new UpdateOrderDefaultValues(status, capture);
            var response = await _httpSender.SendPutHttpsRequestAsync(Environment.GetEnvironmentVariable("ApiV3")! + updateEndPoint, updateOrder, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseV3Update.Root>(response);
            if(jResponse!.Error != null!)
            {
                Console.WriteLine("Doing retry");
                while (counter < 3)
                {
                    counter++;
                    var jResponseUpdate = await SendUpdateRequestAsync(requestHeader, ipn, status, capture, identifier);
                    if (jResponseUpdate!.Error == null!)
                    {
                        return jResponseUpdate;
                    }
                }
            }
            Assert.That(jResponse.ShippingStatus, Is.Not.EqualTo(""));
            Console.WriteLine("SendUpdateRequest Succeeded\n");
            return jResponse;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendUpdateRequest Failed \n" + exception);
            throw;
        }
    }

    public bool ValidateUpdate(ResponseV3Update.Root jResponseRefund, string ipn, string status)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateUpdate");
            Assert.That(jResponseRefund.InstallmentPlanNumber, Is.EqualTo(ipn));
            Assert.That(jResponseRefund.ShippingStatus, Is.EqualTo(status));
            Console.WriteLine("ValidateUpdate Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateUpdate Failed \n" + exception + "\n");
            return false;
        }
    }
}