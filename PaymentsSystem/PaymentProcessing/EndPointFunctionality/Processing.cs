using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.PaymentsSystem.PaymentProcessing.EndPointFunctionality;

public class Processing
{
    private const string ProcessingEndPoint = "/api/PaymentProcessing/processing";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<string> SendGetForProcessingAsync(RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendGetForProcessing");
            await Task.Delay(10*1000);
            var requestUrl = _envConfig.PaymentsUrl + ProcessingEndPoint + "?InstallmentPlanNumebr=" + ipn;
            var response = await _httpSender.SendGetHttpsRequestAsync(requestUrl, requestHeader);
            Console.WriteLine("SendGetForProcessing succeeded\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendGetForProcessing Failed\n" + exception + "\n");
            throw;
        }
    }
}