using Newtonsoft.Json;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;

public class ReceiptFunctionality
{
    private const string EndPoint = "/api/installment-plan/receipt";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<ResponseReceipt.Root> SendGetRequestGetReceiptAsync(
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetRequestGetReceiptAsync");
            var url = _envConfig.AdminUrl + EndPoint + "?InstallmentPlanNumber=" + ipn;
            var response = await _httpSender.SendGetHttpsRequestAsync(url, requestHeader);
            var jResponse = JsonConvert.DeserializeObject<ResponseReceipt.Root>(response);
            Console.WriteLine("Done with SendGetRequestGetReceiptAsync\n");
            return jResponse!;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendGetRequestGetReceiptAsync \n" + exception + "\n");
            throw;
        }
    }
}